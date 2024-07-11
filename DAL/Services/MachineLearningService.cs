using DAL.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Globalization;

namespace DAL.Services
{
    public class MachineLearningService : IMachineLearningService
    {
        private readonly MLContext _mlContext;
        private readonly ILogger<StationDataService> _logger;

        public MachineLearningService(ILogger<StationDataService> logger)
        {
            _mlContext = new MLContext();
            _logger = logger;
        }

        // Load data from CSV files
        public IDataView LoadData(string[] filePaths)
        {
            var data = new List<StationDataCSV>();

            foreach (var filePath in filePaths)
            {
                var lines = File.ReadAllLines(filePath).Skip(1); // Skip header
                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    if (values.Length == 3)
                    {
                        data.Add(new StationDataCSV
                        {
                            timestamp = DateTime.ParseExact(values[0], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                            zone = values[1],
                            total = float.Parse(values[2])
                        });
                    }
                }
            }

            var dataView = _mlContext.Data.LoadFromEnumerable(data);

            // Log the schema
            Console.WriteLine("Schema of the loaded data:");
            foreach (var column in dataView.Schema)
            {
                Console.WriteLine($"{column.Name}: {column.Type}");
            }

            // Print the data
            Console.WriteLine("Data:");
            foreach (var record in data)
            {
                Console.WriteLine($"Timestamp: {record.timestamp}, Zone: {record.zone}, Total: {record.total}");
            }

            return dataView;
        }


        // Train the model without custom mapping
        public ITransformer TrainModel(IDataView data, string modelPath)
        {
            var dataProcessPipeline = _mlContext.Transforms.Conversion.ConvertType("timestamp", "timestamp", DataKind.Single)
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding("ZoneEncoded", "zone"))
                .Append(_mlContext.Transforms.Concatenate("Features", "timestamp", "ZoneEncoded"))
                .Append(_mlContext.Transforms.CopyColumns("Label", "total"));

            var trainer = _mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            var model = trainingPipeline.Fit(data);

            _mlContext.Model.Save(model, data.Schema, modelPath);
            Console.WriteLine($"Model without custom mapping saved to: {modelPath}");

            return model;
        }


        public ITransformer LoadModel(string modelPath)
        {
            DataViewSchema modelSchema;
            try
            {
                var model = _mlContext.Model.Load(modelPath, out modelSchema);
                Console.WriteLine("Model loaded successfully.");
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to load model: {ex.Message}");
                Console.WriteLine($"Failed to load model: {ex.Message}");
                throw;
            }
        }

        // Evaluate the model
        public void EvaluateModel(string[] filePaths, string modelPath)
        {
            var data = LoadData(filePaths);
            var model = LoadModel(modelPath);
            var predictions = model.Transform(data);
            var metrics = _mlContext.Regression.Evaluate(predictions, labelColumnName: "Label", scoreColumnName: "Score");

            _logger.LogInformation($"R^2: {metrics.RSquared}");
            _logger.LogInformation($"Mean Absolute Error: {metrics.MeanAbsoluteError}");
            _logger.LogInformation($"Mean Squared Error: {metrics.MeanSquaredError}");
            _logger.LogInformation($"Root Mean Squared Error: {metrics.RootMeanSquaredError}");
        }

        // Make a prediction
        public StationDataPrediction MakePrediction(string modelPath, StationDataCSV sampleData)
        {
            var model = LoadModel(modelPath);
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<StationDataCSV, StationDataPrediction>(model);
            var prediction = predictionEngine.Predict(sampleData);
            return prediction;
        }

        public class StationDataCSV
        {
            [LoadColumn(0)]
            public DateTime timestamp { get; set; }

            [LoadColumn(1)]
            public string zone { get; set; }

            [LoadColumn(2)]
            public float total { get; set; }
        }

        public class StationDataCSVTransformed
        {
            public float Hour { get; set; }
            public float DayOfWeek { get; set; }
            public string Zone { get; set; }
            public float Total { get; set; }
        }

        public class StationDataPrediction
        {
            [ColumnName("Score")]
            public float total { get; set; }
        }
    }
}