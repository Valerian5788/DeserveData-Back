using DAL.Services;
using Microsoft.ML;

namespace DAL.Interfaces
{
    public interface IMachineLearningService
    {
        IDataView LoadData(string[] filePaths);
        ITransformer TrainModel(IDataView data, string modelPath);
        void EvaluateModel(string[] filePaths, string modelPath);
        MachineLearningService.StationDataPrediction MakePrediction(string modelPath, MachineLearningService.StationDataCSV sampleData);
    }
}
