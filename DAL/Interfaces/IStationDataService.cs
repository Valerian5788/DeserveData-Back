using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IStationDataService
    {
        Task<bool> FetchAndStoreStationsAsync();
        Task<IEnumerable<Station>> GetStationData();
    }
}
