using BLL.Interfaces;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
