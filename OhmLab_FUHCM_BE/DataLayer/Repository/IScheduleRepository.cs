using DataLayer.Entities;

namespace DataLayer.Repository
{
    public interface IScheduleRepository
    {
        Task<List<Schedule>> GetAllAsync();
        Task<Schedule> GetByIdAsync(int id);
        Task<IEnumerable<Schedule>> GetByClassIdAsync(int classId);
        Task<IEnumerable<Schedule>> GetByLecturerIdAsync(Guid lecturerId);
        // Method này giờ sẽ trả về tất cả schedules vì không có bảng Week riêng
        Task<IEnumerable<Schedule>> GetByWeekIdAsync(int weekId);
        Task<IEnumerable<Schedule>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Schedule>> GetByDateAsync(DateTime date);
        Task<Schedule> CreateAsync(Schedule schedule);
        Task<Schedule> UpdateAsync(Schedule schedule);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetScheduleCountByClassAsync(int classId);
    }
} 