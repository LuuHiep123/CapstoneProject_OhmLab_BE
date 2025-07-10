using DataLayer.Entities;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IWeekRepository
    {
        Task<Week> GetByIdAsync(int id);
    }
} 