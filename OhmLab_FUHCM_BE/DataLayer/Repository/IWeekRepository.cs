using DataLayer.Entities;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IWeekRepository
    {
        Task<Week> GetByIdAsync(int id);
        Task<Week> AddAsync(Week week);
        Task<IEnumerable<Week>> GetBySemesterIdAsync(int semesterId);
        Task<IEnumerable<Week>> GetAllAsync();
        Task<Week> UpdateAsync(int id, Week week);
        Task<bool> DeleteAsync(int id);
    }
} 