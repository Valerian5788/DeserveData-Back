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
        public object GetBusStopsAroundStation(double lat, double lon, double radius)
        {
            object retour = _busStopService.GetBusStopsAroundStation(lat, lon, radius).Result;
            string jsonString = JsonSerializer.Serialize(retour, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(jsonString);
            return retour;
        }

        public Task<bool> ImportBusStopsFromMultipleFiles()
        {
            return _busStopService.ImportBusStopsFromMultipleFiles();
        }
    }
}
