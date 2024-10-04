using BLL.Interfaces;
using System.Collections.Generic;
using BLL.Interfaces;
using DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace BLL.Repository
{
    public class FeedbackRepo : IFeedbackRepository
    {
        private readonly IFeedBackService _feedbackService;

        public FeedbackRepo(IFeedBackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public async Task<bool> AddFeedbackAsync(FeedbackModel feedback)
        {
            return await _feedbackService.SubmitFeedbackAsync(feedback);
        }

        public async Task<IEnumerable<FeedbackModel>> GetAllFeedbackAsync()
        {
            return await _feedbackService.GetAllFeedbackAsync();
        }
    }
}