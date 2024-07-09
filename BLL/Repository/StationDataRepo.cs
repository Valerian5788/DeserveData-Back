using BLL.Interfaces;
using DAL.DTO;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Services;
using Microsoft.ML;

namespace BLL.Repository
{
    public class StationDataRepo : IStationDataRepository
    {
        private readonly IStationDataService _stationDataService;

        public StationDataRepo(IStationDataService stationDataService)
        {
            _stationDataService = stationDataService;
        }

        public async Task<bool> FetchAndStoreStationsAsync(string lang)
        {
            return await _stationDataService.FetchAndStoreStationsAsync(lang);
        }

        public async Task<IEnumerable<StationDataDTO>> GetStationsData()
        {
            return await _stationDataService.GetStationData();
        }

        public async Task<bool> FetchAndStoreFacilitiesAsync()
        {
            return await _stationDataService.FetchAndStoreFacilitiesAsync();
        }

        public void TrainModel(string[] filePaths, string modelPath)
        {
            var data = _stationDataService.LoadData(filePaths);
            _stationDataService.TrainModel(data, modelPath);
        }

        public string EvaluateModel(string[] filePaths, string modelPath)
        {
            _stationDataService.EvaluateModel(filePaths, modelPath);
            return "Model evaluated successfully";
        }

        public StationDataService.StationDataPrediction MakePrediction(StationDataService.StationDataCSV sampleData, string modelPath)
        {
            return _stationDataService.MakePrediction(modelPath, sampleData);
        }
    }
}