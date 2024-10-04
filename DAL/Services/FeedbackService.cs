using DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using DAL.AppDbContextFolder;

namespace DAL.Services
{
    public class FeedbackService : IFeedBackService
    {
        private readonly AppDbContext _context;

        public FeedbackService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SubmitFeedbackAsync(FeedbackModel feedback)
        {
            _context.Feedbacks.Add(feedback);
            var savedEntries = await _context.SaveChangesAsync();
            return savedEntries > 0;
        }

        public async Task<IEnumerable<FeedbackModel>> GetAllFeedbackAsync()
        {
            return await _context.Feedbacks.ToListAsync();
        }
    }
}