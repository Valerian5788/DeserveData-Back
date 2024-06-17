using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeserveData_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : Controller
    {
        private readonly IStationDataRepository _stationDataRepository;

        public DataController(IStationDataRepository stationDataRepository)
        {
            _stationDataRepository = stationDataRepository;
        }

        [HttpGet]
        public async Task<IActionResult> FetchStationsData()
        {
            bool result = await _stationDataRepository.FetchAndStoreStationsAsync();
            return result ? Ok("Data fetched and stored successfully") : BadRequest("Failed to fetch and store data");
        }

    }
}
