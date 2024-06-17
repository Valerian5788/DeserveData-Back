using BLL.Interfaces;
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
    }
}
