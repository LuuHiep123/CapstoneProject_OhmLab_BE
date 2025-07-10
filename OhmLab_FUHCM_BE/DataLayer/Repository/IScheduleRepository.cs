using DataLayer.Entities;

namespace DataLayer.Repository
{
    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>> GetAllAsync();
        Task<Schedule> GetByIdAsync(int id);
        Task<IEnumerable<Schedule>> GetByClassIdAsync(int classId);
        Task<IEnumerable<Schedule>> GetByLecturerIdAsync(Guid lecturerId);
        Task<IEnumerable<Schedule>> GetByWeekIdAsync(int weekId);
        Task<Schedule> CreateAsync(Schedule schedule);
        Task<Schedule> UpdateAsync(Schedule schedule);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetScheduleCountByClassAsync(int classId);
    }
} 