using DAL.Entities;

namespace BLL.Interfaces
{
    public interface IStationDataRepository
    {
        Task<bool> FetchAndStoreStationsAsync(string lang);

        Task<IEnumerable<Station>> GetStationsData();
    }
}
