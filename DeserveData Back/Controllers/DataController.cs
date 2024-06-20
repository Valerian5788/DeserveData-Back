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

        [HttpPost("StoreStationData/{lang}")]
        public async Task<IActionResult> FetchStationsData_Fr(string lang)
        {
            bool result = await _stationDataRepository.FetchAndStoreStationsAsync(lang);
            return result ? Ok("Data fetched and stored successfully") : BadRequest("Failed to fetch and store data");
        }
        

        [HttpGet("GetStationsData")]
        public async Task<IActionResult> GetStationsData()
        {
            var stations = await _stationDataRepository.GetStationsData();
            return stations != null ? Ok(stations) : NotFound();
        }

        [HttpPost("StoreFacilitiesData")]   
        public async Task<IActionResult> FetchFacilitiesData()
        {
            bool result = await _stationDataRepository.FetchAndStoreFacilitiesAsync();
            return result ? Ok("Data fetched and stored successfully") : BadRequest("Failed to fetch and store data");
        }

    }
}
