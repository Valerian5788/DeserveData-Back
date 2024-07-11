using DAL.DTO;
using DAL.Entities;
using DAL.Services;
using Microsoft.ML;

namespace BLL.Interfaces
{
    public interface IStationDataRepository
    {
        Task<bool> FetchAndStoreStationsAsync(string lang);
        Task<IEnumerable<StationDataDTO>> GetStationsData();
        Task<bool> FetchAndStoreFacilitiesAsync();


    }
}
