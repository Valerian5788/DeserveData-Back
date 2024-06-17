using DAL.Entities;

namespace BLL.Interfaces
{
    public interface IStationDataRepository
    {
        Task<bool> FetchAndStoreStationsAsync();

        Task<IEnumerable<Station>> GetStationsData();
    }
}
