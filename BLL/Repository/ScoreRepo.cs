using BLL.Interfaces;
using DAL.Interfaces;
using System.Threading.Tasks;

namespace BLL.Repository
{
    public class ScoreRepo : IScoreRepository
    {
        private readonly IScoreService _scoreService;

        public ScoreRepo(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        public async Task<double> GetStationScore(int stationId)
        {
            return await _scoreService.CalculateStationScore(stationId);
        }
    }
}