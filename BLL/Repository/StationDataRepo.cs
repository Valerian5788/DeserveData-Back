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


        public async Task<bool> FetchAndStoreStationsAsync()
        {
            return await _stationDataService.FetchAndStoreStationsAsync();
        }

        public async Task<IEnumerable<Station>> GetStationsData()
        {
            return await _stationDataService.GetStationData();
        }
    }
}
