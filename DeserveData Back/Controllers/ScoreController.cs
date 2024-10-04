using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using System.Threading.Tasks;

namespace DeserveData_Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreController : ControllerBase
    {
        private readonly IScoreRepository _scoreRepository;

        public ScoreController(IScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
        }

        [HttpGet("{stationId}")]
        public async Task<IActionResult> GetStationScore(int stationId)
        {
            var score = await _scoreRepository.GetStationScore(stationId);
            return Ok(new { StationId = stationId, Score = score });
        }
    }
}