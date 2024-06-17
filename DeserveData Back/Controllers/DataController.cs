using BLL.Interfaces;
using DAL.Entities;
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

        [HttpGet("StoreStationData")]
        public async Task<IActionResult> FetchStationsData()
        {
            bool result = await _stationDataRepository.FetchAndStoreStationsAsync();
            return result ? Ok("Data fetched and stored successfully") : BadRequest("Failed to fetch and store data");
        }

        [HttpGet("GetStationsData")]
        public async Task<IActionResult> GetStationsData()
        {
            var stations = await _stationDataRepository.GetStationsData();
            return stations != null ? Ok(stations) : NotFound();
        }

    }
}
