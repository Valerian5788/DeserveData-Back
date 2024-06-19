using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;

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

        public async Task<IEnumerable<Station>> GetStationsData()
        {
            return await _stationDataService.GetStationData();
        }
    }
}
