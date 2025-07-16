using DataLayer.Entities;

namespace DataLayer.Repository
{
    public interface IGradeRepository
    {
        Task<IEnumerable<Grade>> GetAllAsync();
        Task<Grade> GetByIdAsync(int id);
        Task<IEnumerable<Grade>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Grade>> GetByLabIdAsync(int labId);
        Task<IEnumerable<Grade>> GetByTeamIdAsync(int teamId);
        Task<IEnumerable<Grade>> GetByStatusAsync(string status);
        Task<Grade> CreateAsync(Grade grade);
        Task<Grade> UpdateAsync(Grade grade);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<decimal> GetAverageScoreByLabAsync(int labId);
        Task<decimal> GetAverageScoreByUserAsync(Guid userId);
    }
} 