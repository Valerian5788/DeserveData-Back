using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult SeeBusStopAroundStation(double lat, double lon, double radius)
        {
            return Ok(_busStopRepository.GetBusStopsAroundStation(lat, lon, radius));
        }

        [HttpPost("FetchDataTec")]
        [Authorize("Admin")]
        public async Task<IActionResult> ImportBusStops()
        {
            bool success = await _busStopRepository.ImportBusStopsFromMultipleFiles();
            return success ? Ok("Data fetched and stored successfully") : BadRequest("Failed to fetch and store data");
        }

    }
}
