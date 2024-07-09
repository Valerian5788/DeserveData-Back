using DAL.DTO;
using DAL.Entities;
using DAL.Services;
using Microsoft.ML;

namespace DAL.Interfaces
{
    public interface IStationDataService
    {
        Task<bool> FetchAndStoreStationsAsync(string lang);
        Task<IEnumerable<StationDataDTO>> GetStationData();
        Task<bool> FetchAndStoreFacilitiesAsync();
        IDataView LoadData(string[] filePaths);
        ITransformer TrainModel(IDataView data, string modelPath);
        void EvaluateModel(string[] filePaths, string modelPath);
        StationDataService.StationDataPrediction MakePrediction(string modelPath, StationDataService.StationDataCSV sampleData);
    }
}
