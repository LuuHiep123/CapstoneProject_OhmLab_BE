using DataLayer.DBContext;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository.Implement
{
    public class ReportRepository : IReportRepository
    {
        private readonly db_abadcb_ohmlabContext _DBContext;

        public ReportRepository(db_abadcb_ohmlabContext OhmLab_DBContext)
        {
            _DBContext = OhmLab_DBContext;
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            try
            {
                return await _DBContext.Reports
                    .Include(r => r.User)
                    .Include(r => r.Schedule)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Report> GetByIdAsync(int id)
        {
            try
            {
                return await _DBContext.Reports
                    .Include(r => r.User)
                    .Include(r => r.Schedule)
                    .FirstOrDefaultAsync(r => r.ReportId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Report>> GetByUserIdAsync(Guid userId)
        {
            try
            {
                return await _DBContext.Reports
                    .Include(r => r.User)
                    .Include(r => r.Schedule)
                    .Where(r => r.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Report>> GetByScheduleIdAsync(int scheduleId)
        {
            try
            {
                return await _DBContext.Reports
                    .Include(r => r.User)
                    .Include(r => r.Schedule)
                    .Where(r => r.ScheduleId == scheduleId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Report>> GetByStatusAsync(string status)
        {
            try
            {
                return await _DBContext.Reports
                    .Include(r => r.User)
                    .Include(r => r.Schedule)
                    .Where(r => r.ReportStatus == status)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Report>> GetReportsByStudentAsync(Guid studentId)
        {
            try
            {
                return await _DBContext.Reports
                    .Include(r => r.User)
                    .Include(r => r.Schedule)
                    .Where(r => r.UserId == studentId && r.User.UserRoleName == "Student")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Report>> GetReportsByScheduleAsync(int scheduleId)
        {
            try
            {
                return await _DBContext.Reports
                    .Include(r => r.User)
                    .Include(r => r.Schedule)
                    .Where(r => r.ScheduleId == scheduleId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Report>> GetUngradedReportsAsync()
        {
            try
            {
                // Lấy các report chưa được chấm điểm (chưa có Grade tương ứng)
                var reports = await _DBContext.Reports
                    .Include(r => r.User)
                    .Include(r => r.Schedule)
                    .Where(r => r.ReportStatus == "Submitted")
                    .ToListAsync();

                var ungradedReports = new List<Report>();
                foreach (var report in reports)
                {
                    var hasGrade = await _DBContext.Grades
                        .AnyAsync(g => g.UserId == report.UserId && g.LabId == report.Schedule.Class.SubjectId);
                    
                    if (!hasGrade)
                    {
                        ungradedReports.Add(report);
                    }
                }

                return ungradedReports;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Report> CreateAsync(Report report)
        {
            try
            {
                await _DBContext.Reports.AddAsync(report);
                await _DBContext.SaveChangesAsync();
                return report;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Report> UpdateAsync(Report report)
        {
            try
            {
                _DBContext.Reports.Update(report);
                await _DBContext.SaveChangesAsync();
                return report;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var report = await GetByIdAsync(id);
                if (report != null)
                {
                    _DBContext.Reports.Remove(report);
                    await _DBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _DBContext.Reports.AnyAsync(r => r.ReportId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetReportCountByScheduleAsync(int scheduleId)
        {
            try
            {
                return await _DBContext.Reports
                    .CountAsync(r => r.ScheduleId == scheduleId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetReportCountByStudentAsync(Guid studentId)
        {
            try
            {
                return await _DBContext.Reports
                    .CountAsync(r => r.UserId == studentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // New methods for Incident Management
        public async Task<IEnumerable<Report>> GetIncidentReportsAsync()
        {
            try
            {
                return await _DBContext.Reports
                    .Include(r => r.User)
                    .Include(r => r.Schedule)
                    .Where(r => r.ReportTitle.Contains("Chập mạch") ||
                                r.ReportTitle.Contains("Thiết bị hỏng") ||
                                r.ReportTitle.Contains("Tai nạn") ||
                                r.ReportTitle.Contains("Sự cố") ||
                                r.ReportTitle.Contains("Hỏng") ||
                                r.ReportTitle.Contains("Lỗi"))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Report>> GetIncidentReportsByStatusAsync(string status)
        {
            try
            {
                return await _DBContext.Reports
                    .Include(r => r.User)
                    .Include(r => r.Schedule)
                    .Where(r => r.ReportStatus == status &&
                                (r.ReportTitle.Contains("Chập mạch") ||
                                 r.ReportTitle.Contains("Thiết bị hỏng") ||
                                 r.ReportTitle.Contains("Tai nạn") ||
                                 r.ReportTitle.Contains("Sự cố") ||
                                 r.ReportTitle.Contains("Hỏng") ||
                                 r.ReportTitle.Contains("Lỗi")))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Report>> GetReportsByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return await _DBContext.Reports
                    .Include(r => r.User)
                    .Include(r => r.Schedule)
                    .Where(r => r.ReportCreateDate >= fromDate && r.ReportCreateDate <= toDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Report>> GetReportsByUserAndStatusAsync(Guid userId, string status)
        {
            try
            {
                return await _DBContext.Reports
                    .Include(r => r.User)
                    .Include(r => r.Schedule)
                    .Where(r => r.UserId == userId && r.ReportStatus == status)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
} 