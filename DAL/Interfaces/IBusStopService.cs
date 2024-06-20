using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IBusStopService
    {
        Task<object> GetBusStopsAroundStation(string city_name, double lat, double lon, double radius);
    }
}
