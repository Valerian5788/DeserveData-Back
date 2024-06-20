using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeserveData_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusStopController : Controller
    {
        private readonly IBusStopRepository _busStopRepository;
        public BusStopController(IBusStopRepository busStopRepository)
        {
            _busStopRepository = busStopRepository;
        }


        [HttpGet]
        public IActionResult SeeBusStopAroundStation(string city_name, double lat, double lon, double radius)
        {
            return Ok(_busStopRepository.GetBusStopsAroundStation(city_name, lat, lon, radius));
        }

    }
}
