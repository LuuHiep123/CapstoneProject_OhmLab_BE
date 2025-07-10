using DataLayer.Entities;

namespace DataLayer.Repository
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAllAsync();
        Task<Report> GetByIdAsync(int id);
        Task<IEnumerable<Report>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Report>> GetByScheduleIdAsync(int scheduleId);
        Task<IEnumerable<Report>> GetByStatusAsync(string status);
        Task<IEnumerable<Report>> GetReportsByStudentAsync(Guid studentId);
        Task<IEnumerable<Report>> GetReportsByScheduleAsync(int scheduleId);
        Task<IEnumerable<Report>> GetUngradedReportsAsync();
        Task<Report> CreateAsync(Report report);
        Task<Report> UpdateAsync(Report report);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetReportCountByScheduleAsync(int scheduleId);
        Task<int> GetReportCountByStudentAsync(Guid studentId);
    }
} 