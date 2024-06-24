using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeserveData_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : Controller
    {
        private readonly IPlatformHeightRepository _platformHeightRepository;

        public PlatformController(IPlatformHeightRepository platformHeightRepository)
        {
            _platformHeightRepository = platformHeightRepository; 
        }

        [HttpPost("FetchData")]
        public async Task<IActionResult> FetchAndStoreAllPlatformData()
        {
            bool result = await _platformHeightRepository.FetchAndStoreAllPlatformData(); 
            return result ? Ok("Data fetched and stored successfully") : BadRequest("Failed to fetch and store data");    
        }

        [HttpGet("GetAllPlatforms")]
        public async Task<IActionResult> GetAllPlatforms()
        {
            var platforms = await _platformHeightRepository.GetAllPlatforms();
            return platforms != null ? Ok(platforms) : NotFound("No platforms found");
        }
    }
}
