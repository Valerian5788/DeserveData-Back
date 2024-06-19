using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IStationDataService
    {
        Task<bool> FetchAndStoreStationsAsync(string lang);
        Task<IEnumerable<Station>> GetStationData();
        Task<bool> FetchAndStoreFacilitiesAsync();
    }
}
