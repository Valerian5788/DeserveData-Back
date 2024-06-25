using DAL.DTO;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IStationDataService
    {
        Task<bool> FetchAndStoreStationsAsync(string lang);
        Task<IEnumerable<StationDataDTO>> GetStationData();
        Task<bool> FetchAndStoreFacilitiesAsync();
    }
}
