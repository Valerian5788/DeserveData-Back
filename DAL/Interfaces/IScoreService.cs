using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IScoreService
    {
        Task<double> CalculateStationScore(int stationId);
    }
}