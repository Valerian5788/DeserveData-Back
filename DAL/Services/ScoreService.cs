using DAL.Interfaces;
using DAL.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.AppDbContextFolder;
using DAL.Services;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace DAL.Services
{
    public class ScoreService : IScoreService
    {
        private readonly AppDbContext _context;
        private readonly IMachineLearningService _mlRepository;
        private readonly string _modelPath = "C:\\Users\\User\\Documents\\DeserveData\\Back\\DAL\\Intern Data\\model.zip";

        public ScoreService(AppDbContext context, IMachineLearningService mlRepository)
        {
            _context = context;
            _mlRepository = mlRepository;
        }

        public async Task<double> CalculateStationScore(int stationId, DateTime predictionTime)
        {
            var station = await _context.stations
                .Include(s => s.Facilities)
                .Include(s => s.Platforms)
                .FirstOrDefaultAsync(s => s.Id_Station == stationId);

            if (station == null)
                return 0;

            double score = 0;

            // Facilities score (40% of total)
            score += CalculateFacilitiesScore(station.Facilities) * 4;

            // Feedback score (25% of total)
            var feedbackScore = await CalculateFeedbackScore(stationId);
            score += feedbackScore * 2.5;

            // Platform accessibility score (20% of total)
            score += CalculatePlatformScore(station.Platforms) * 2;

            // Occupancy prediction score (15% of total)
            var occupancyScore = await CalculateOccupancyScore(station.name_fr, predictionTime);
            score += occupancyScore * 1.5;

            return Math.Round(score, 1);
        }

        private double CalculateFacilitiesScore(Facilities facilities)
        {
            double score = 0;
            if (facilities.PMRToilets) score += 0.25;
            if (facilities.Escalator) score += 0.25;
            if (facilities.LiftOnPlatform) score += 0.25;
            if (facilities.PMRAssistance) score += 0.25;

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

        private async Task<double> CalculateOccupancyScore(string stationName, DateTime predictionTime)
        {
            var sampleData = new MachineLearningService.StationDataCSV
            {
                timestamp = predictionTime,
                zone = stationName,
                total = 0 // This value doesn't matter for prediction
            };

            var prediction = _mlRepository.MakePrediction(_modelPath, sampleData);

            // Normalize the prediction to a 0-1 scale
            // Assuming maximum occupancy is 1000 (adjust this based on your data)
            const double maxOccupancy = 1000;
            double normalizedOccupancy = Math.Min(prediction.total / maxOccupancy, 1);

            // Invert the score so that lower occupancy results in a higher score
            return 1 - normalizedOccupancy;
        }
    }
}