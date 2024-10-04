using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IScoreRepository
    {
        Task<double> GetStationScore(int stationId);
    }
}