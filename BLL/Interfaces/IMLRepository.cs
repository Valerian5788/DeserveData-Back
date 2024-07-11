using DAL.Services;

namespace BLL.Interfaces
{
    public interface IMLRepository
    {
        void TrainModel(string[] filePaths, string modelPath);
        string EvaluateModel(string[] filePaths, string modelPath);
        MachineLearningService.StationDataPrediction MakePrediction(MachineLearningService.StationDataCSV sampleData, string modelPath);
    }
}
