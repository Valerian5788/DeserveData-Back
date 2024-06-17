namespace BLL.Interfaces
{
    public interface IStationDataRepository
    {
        Task<bool> FetchAndStoreStationsAsync();
    }
}
