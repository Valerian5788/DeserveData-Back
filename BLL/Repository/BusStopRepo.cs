using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Repository
{
    public class BusStopRepo : IBusStopRepository
    {
        private readonly IBusStopService _busStopService;
        public BusStopRepo(IBusStopService BusStopService)
        {
            _busStopService = BusStopService;
        }
        public object GetBusStopsAroundStation(string city_name, double lat, double lon, double radius)
        {
            object retour = _busStopService.GetBusStopsAroundStation(city_name, lat, lon, radius).Result;
            string jsonString = JsonSerializer.Serialize(retour, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(jsonString);
            return retour;
        }
    }
}
