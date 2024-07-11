using BLL.Interfaces;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeserveData_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MLController : Controller
    {
        private readonly IMLRepository _MachineLearningRepository;
        private readonly string _modelPath = "C:\\Users\\User\\Documents\\DeserveData\\Back\\DAL\\Intern Data\\model.zip"; // Define the model path

        public MLController(IMLRepository machineLearningRepository)
        {
            _MachineLearningRepository = machineLearningRepository;
        }

        [HttpPost("TrainModel")]
        public IActionResult TrainModel([FromBody] string[] filePaths)
        {
            _MachineLearningRepository.TrainModel(filePaths, _modelPath);
            return Ok("Model trained and saved.");
        }


        [HttpPost("EvaluateModel")]
        public IActionResult EvaluateModel([FromBody] string[] filePaths)
        {
            var result = _MachineLearningRepository.EvaluateModel(filePaths, _modelPath);
            return Ok(result);
        }

        [HttpPost("MakePrediction")]
        public IActionResult MakePrediction([FromBody] MachineLearningService.StationDataCSV sampleData)
        {
            var prediction = _MachineLearningRepository.MakePrediction(sampleData, _modelPath);
            return Ok(prediction);
        }
    }
}
