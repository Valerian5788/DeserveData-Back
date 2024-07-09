using DAL.DTO;
using DAL.Entities;
using DAL.Services;
using Microsoft.ML;

namespace BLL.Interfaces
{
    public interface IStationDataRepository
    {
        Task<bool> FetchAndStoreStationsAsync(string lang);
        Task<IEnumerable<StationDataDTO>> GetStationsData();
        Task<bool> FetchAndStoreFacilitiesAsync();
        void TrainModel(string[] filePaths, string modelPath);
        string EvaluateModel(string[] filePaths, string modelPath);
        StationDataService.StationDataPrediction MakePrediction(StationDataService.StationDataCSV sampleData, string modelPath);
    }
}
