using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IFeedBackService
    {
        Task<bool> SubmitFeedbackAsync(FeedbackModel feedback);
        Task<IEnumerable<FeedbackModel>> GetAllFeedbackAsync();
    }
}