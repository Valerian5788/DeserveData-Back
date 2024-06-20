using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBusStopRepository
    {
        object GetBusStopsAroundStation(string city_name, double lat, double lon, double radius);
    }
}
