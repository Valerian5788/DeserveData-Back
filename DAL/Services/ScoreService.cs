using DAL.Interfaces;
using DAL.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.AppDbContextFolder;

namespace DAL.Services
{
    public class ScoreService : IScoreService
    {
        private readonly AppDbContext _context;

        public ScoreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<double> CalculateStationScore(int stationId)
        {
            var station = await _context.stations
                .Include(s => s.Facilities)
                .Include(s => s.Platforms)
                .FirstOrDefaultAsync(s => s.Id_Station == stationId);

            if (station == null)
                return 0;

            double score = 0;

            // Facilities score (50% of total)
            score += CalculateFacilitiesScore(station.Facilities) * 5;

            // Feedback score (30% of total)
            var feedbackScore = await CalculateFeedbackScore(stationId);
            score += feedbackScore * 3;

            // Platform accessibility score (20% of total)
            score += CalculatePlatformScore(station.Platforms) * 2;

            return Math.Round(score, 1);
        }

        private double CalculateFacilitiesScore(Facilities facilities)
        {
            double score = 0;
            if (facilities.PMRToilets) score += 0.2;
            if (facilities.Escalator) score += 0.2;
            if (facilities.LiftOnPlatform) score += 0.3;
            if (facilities.PMRAssistance) score += 0.3;

            return score;
        }

        private async Task<double> CalculateFeedbackScore(int stationId)
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.StationId == stationId)
                .ToListAsync();

            if (feedbacks.Count == 0)
                return 0;

            return feedbacks.Average(f => f.Rating) / 5.0;  // Assuming rating is out of 5
        }

        private double CalculatePlatformScore(ICollection<Platforms> platforms)
        {
            if (platforms == null || platforms.Count == 0)
                return 0;

            return platforms.Count(p => p.Hauteur == "high") / (double)platforms.Count;
        }
    }
}