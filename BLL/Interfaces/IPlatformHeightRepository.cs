﻿using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IPlatformHeightRepository
    {
        Task<bool> FetchAndStoreAllPlatformData();
        Task<List<Platforms>> GetAllPlatforms();
    }
}
