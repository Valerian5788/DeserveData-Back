using DAL.DTO;
using DAL.Entities;

namespace BLL.Interfaces
{
    public interface IStationDataRepository
    {
        Task<bool> FetchAndStoreStationsAsync(string lang);

        Task<IEnumerable<StationDataDTO>> GetStationsData();
        Task<bool> FetchAndStoreFacilitiesAsync();
    }
}
