using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace BLL.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<bool> AddFeedbackAsync(FeedbackModel feedback);
        Task<IEnumerable<FeedbackModel>> GetAllFeedbackAsync();
    }
}