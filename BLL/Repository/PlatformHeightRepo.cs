using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;


namespace BLL.Repository
{
    public class PlatformHeightRepo : IPlatformHeightRepository
    {
        private readonly IPlatformHeightService _platformHeightService;
        public PlatformHeightRepo(IPlatformHeightService platformHeightService)
        {
            _platformHeightService = platformHeightService;
        }
        public Task<bool> FetchAndStoreAllPlatformData()
        {
            return _platformHeightService.FetchAndStoreAllPlatformData();
        }

        public Task<List<Platforms>> GetAllPlatforms()
        {
            return _platformHeightService.GetAllPlatforms();
        }
    }
}
