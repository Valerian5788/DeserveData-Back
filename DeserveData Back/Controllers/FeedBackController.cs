using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using System.Threading.Tasks;
using DAL.Entities;

namespace DeserveData_Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedBackController(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackModel feedback)
        {
            var result = await _feedbackRepository.AddFeedbackAsync(feedback);
            if (result)
                return Ok("Feedback submitted successfully");
            return BadRequest("Failed to submit feedback");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFeedback()
        {
            var feedbacks = await _feedbackRepository.GetAllFeedbackAsync();
            return Ok(feedbacks);
        }
    }
}