using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Services;


namespace BLL.Repository
{
    public class MachineLearningRepo : IMLRepository
    {
        private readonly IMachineLearningService _machineLearningService;

        public MachineLearningRepo(IMachineLearningService machineLearningService)
        {
            _machineLearningService = machineLearningService;
        }

        public void TrainModel(string[] filePaths, string modelPath)
        {
            var data = _machineLearningService.LoadData(filePaths);
            _machineLearningService.TrainModel(data, modelPath);
        }

        public string EvaluateModel(string[] filePaths, string modelPath)
        {
            _machineLearningService.EvaluateModel(filePaths, modelPath);
            return "Model evaluated successfully";
        }

        public MachineLearningService.StationDataPrediction MakePrediction(MachineLearningService.StationDataCSV sampleData, string modelPath)
        {
            return _machineLearningService.MakePrediction(modelPath, sampleData);
        }
    }
}
