namespace DAL.Interfaces
{
    public interface IStationDataService
    {
        Task<bool> FetchAndStoreStationsAsync();
    }
}
