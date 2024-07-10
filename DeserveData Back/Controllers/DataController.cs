using BLL.Interfaces;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeserveData_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : Controller
    {
        private readonly IStationDataRepository _stationDataRepository;
        private readonly string _modelPath = "C:\\Users\\User\\Documents\\DeserveData\\Back\\DAL\\Intern Data\\model.zip"; // Define the model path

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
        [HttpPost("TrainModel")]
        public IActionResult TrainModel([FromBody] string[] filePaths)
        {
            _stationDataRepository.TrainModel(filePaths, _modelPath);
            return Ok("Model trained and saved.");
        }


        [HttpPost("EvaluateModel")]
        public IActionResult EvaluateModel([FromBody] string[] filePaths)
        {
            var result = _stationDataRepository.EvaluateModel(filePaths, _modelPath);
            return Ok(result);
        }

        [HttpPost("MakePrediction")]
        public IActionResult MakePrediction([FromBody] StationDataService.StationDataCSV sampleData)
        {
            var prediction = _stationDataRepository.MakePrediction(sampleData, _modelPath);
            return Ok(prediction);
        }

    }
}
