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
                var reports = await _DBContext.Reports
                    .Include(r => r.User)
                    .ToListAsync();

                // Load RegistrationSchedule separately to handle null values safely
                foreach (var report in reports)
                {
                    if (report.RegistrationScheduleId.HasValue)
                    {
                        await _DBContext.Entry(report)
                            .Reference(r => r.RegistrationSchedule)
                            .Query()
                            .Include(rs => rs.Slot)
                            .Include(rs => rs.Class)
                            .Include(rs => rs.User)
                            .LoadAsync();
                    }
                }

                return reports;
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
                var report = await _DBContext.Reports
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.ReportId == id);

                if (report != null && report.RegistrationScheduleId.HasValue)
                {
                    await _DBContext.Entry(report)
                        .Reference(r => r.RegistrationSchedule)
                        .Query()
                        .Include(rs => rs.Slot)
                        .Include(rs => rs.Class)
                        .Include(rs => rs.User)
                        .LoadAsync();
                }

                return report;
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
                    .Include(r => r.RegistrationSchedule)
                    .Include(r => r.Schedule)
                    .Where(r => r.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Report>> GetByRegistrationScheduleIdAsync(int registrationScheduleId)
        {
            try
            {
                var reports = await _DBContext.Reports
                    .Include(r => r.User)
                    .Where(r => r.RegistrationScheduleId == registrationScheduleId)
                    .ToListAsync();

                // Load RegistrationSchedule separately to handle null values safely
                foreach (var report in reports)
                {
                    if (report.RegistrationScheduleId.HasValue)
                    {
                        await _DBContext.Entry(report)
                            .Reference(r => r.RegistrationSchedule)
                            .Query()
                            .Include(rs => rs.Slot)
                            .Include(rs => rs.Class)
                            .Include(rs => rs.User)
                            .LoadAsync();
                    }
                }

                return reports;
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
                var reports = await _DBContext.Reports
                    .Include(r => r.User)
                    .Where(r => r.ReportStatus == status)
                    .ToListAsync();

                // Load RegistrationSchedule separately to handle null values safely
                foreach (var report in reports)
                {
                    if (report.RegistrationScheduleId.HasValue)
                    {
                        await _DBContext.Entry(report)
                            .Reference(r => r.RegistrationSchedule)
                            .Query()
                            .Include(rs => rs.Slot)
                            .Include(rs => rs.Class)
                            .Include(rs => rs.User)
                            .LoadAsync();
                    }
                }

                return reports;
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
                var reports = await _DBContext.Reports
                    .Include(r => r.User)
                    .Where(r => r.UserId == studentId && r.User.UserRoleName == "Student")
                    .ToListAsync();

                // Load RegistrationSchedule separately to handle null values safely
                foreach (var report in reports)
                {
                    if (report.RegistrationScheduleId.HasValue)
                    {
                        await _DBContext.Entry(report)
                            .Reference(r => r.RegistrationSchedule)
                            .Query()
                            .Include(rs => rs.Slot)
                            .Include(rs => rs.Class)
                            .Include(rs => rs.User)
                            .LoadAsync();
                    }
                }

                return reports;
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
                    .Where(r => r.ReportStatus == "Submitted")
                    .ToListAsync();

                // Load RegistrationSchedule separately to handle null values safely
                foreach (var report in reports)
                {
                    if (report.RegistrationScheduleId.HasValue)
                    {
                        await _DBContext.Entry(report)
                            .Reference(r => r.RegistrationSchedule)
                            .Query()
                            .Include(rs => rs.Slot)
                            .Include(rs => rs.Class)
                            .Include(rs => rs.User)
                            .LoadAsync();
                    }
                }

                var ungradedReports = new List<Report>();
                foreach (var report in reports)
                {
                    // Check if RegistrationSchedule exists and has Class
                    if (report.RegistrationSchedule?.Class != null)
                    {
                        var hasGrade = await _DBContext.Grades
                            .AnyAsync(g => g.UserId == report.UserId && g.LabId == report.RegistrationSchedule.Class.SubjectId);
                        
                        if (!hasGrade)
                        {
                            ungradedReports.Add(report);
                        }
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
                var reports = await _DBContext.Reports
                    .Include(r => r.User)
                    .Where(r => r.ReportTitle.Contains("Chập mạch") ||
                                r.ReportTitle.Contains("Thiết bị hỏng") ||
                                r.ReportTitle.Contains("Tai nạn") ||
                                r.ReportTitle.Contains("Sự cố") ||
                                r.ReportTitle.Contains("Hỏng") ||
                                r.ReportTitle.Contains("Lỗi"))
                    .ToListAsync();

                // Load RegistrationSchedule separately to handle null values safely
                foreach (var report in reports)
                {
                    if (report.RegistrationScheduleId.HasValue)
                    {
                        await _DBContext.Entry(report)
                            .Reference(r => r.RegistrationSchedule)
                            .Query()
                            .Include(rs => rs.Slot)
                            .Include(rs => rs.Class)
                            .Include(rs => rs.User)
                            .LoadAsync();
                    }
                }

                return reports;
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
                var reports = await _DBContext.Reports
                    .Include(r => r.User)
                    .Where(r => r.ReportStatus == status &&
                                (r.ReportTitle.Contains("Chập mạch") ||
                                 r.ReportTitle.Contains("Thiết bị hỏng") ||
                                 r.ReportTitle.Contains("Tai nạn") ||
                                 r.ReportTitle.Contains("Sự cố") ||
                                 r.ReportTitle.Contains("Hỏng") ||
                                 r.ReportTitle.Contains("Lỗi")))
                    .ToListAsync();

                // Load RegistrationSchedule separately to handle null values safely
                foreach (var report in reports)
                {
                    if (report.RegistrationScheduleId.HasValue)
                    {
                        await _DBContext.Entry(report)
                            .Reference(r => r.RegistrationSchedule)
                            .Query()
                            .Include(rs => rs.Slot)
                            .Include(rs => rs.Class)
                            .Include(rs => rs.User)
                            .LoadAsync();
                    }
                }

                return reports;
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
                var reports = await _DBContext.Reports
                    .Include(r => r.User)
                    .Where(r => r.ReportCreateDate >= fromDate && r.ReportCreateDate <= toDate)
                    .ToListAsync();

                // Load RegistrationSchedule separately to handle null values safely
                foreach (var report in reports)
                {
                    if (report.RegistrationScheduleId.HasValue)
                    {
                        await _DBContext.Entry(report)
                            .Reference(r => r.RegistrationSchedule)
                            .Query()
                            .Include(rs => rs.Slot)
                            .Include(rs => rs.Class)
                            .Include(rs => rs.User)
                            .LoadAsync();
                    }
                }

                return reports;
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
                var reports = await _DBContext.Reports
                    .Include(r => r.User)
                    .Where(r => r.UserId == userId && r.ReportStatus == status)
                    .ToListAsync();

                // Debug từng record để tìm ra vấn đề
                foreach (var report in reports)
                {
                    if (report.RegistrationScheduleId.HasValue)
                    {
                        try
                        {
                            Console.WriteLine($"=== DEBUG ReportId: {report.ReportId}, RegistrationScheduleId: {report.RegistrationScheduleId} ===");
                            
                            // Bước 1: Load RegistrationSchedule cơ bản
                            await _DBContext.Entry(report).Reference(r => r.RegistrationSchedule).LoadAsync();
                            Console.WriteLine("✅ Load RegistrationSchedule cơ bản thành công");
                            
                            if (report.RegistrationSchedule != null)
                            {
                                Console.WriteLine($"- SlotId: {report.RegistrationSchedule.SlotId}");
                                Console.WriteLine($"- ClassId: {report.RegistrationSchedule.ClassId}");
                                Console.WriteLine($"- TeacherId: {report.RegistrationSchedule.TeacherId}");
                                Console.WriteLine($"- LabId: {report.RegistrationSchedule.LabId}");
                                
                                // Bước 2: Test load từng navigation property riêng biệt
                                try
                                {
                                    await _DBContext.Entry(report.RegistrationSchedule).Reference(rs => rs.Slot).LoadAsync();
                                    Console.WriteLine("✅ Load Slot thành công");
                                }
                                catch (Exception slotEx)
                                {
                                    Console.WriteLine($"❌ Lỗi khi load Slot: {slotEx.Message}");
                                }
                                
                                try
                                {
                                    await _DBContext.Entry(report.RegistrationSchedule).Reference(rs => rs.Class).LoadAsync();
                                    Console.WriteLine("✅ Load Class thành công");
                                }
                                catch (Exception classEx)
                                {
                                    Console.WriteLine($"❌ Lỗi khi load Class: {classEx.Message}");
                                }
                                
                                try
                                {
                                    await _DBContext.Entry(report.RegistrationSchedule).Reference(rs => rs.User).LoadAsync();
                                    Console.WriteLine("✅ Load User (Teacher) thành công");
                                }
                                catch (Exception userEx)
                                {
                                    Console.WriteLine($"❌ Lỗi khi load User (Teacher): {userEx.Message}");
                                }
                                
                                try
                                {
                                    await _DBContext.Entry(report.RegistrationSchedule).Reference(rs => rs.Lab).LoadAsync();
                                    Console.WriteLine("✅ Load Lab thành công");
                                }
                                catch (Exception labEx)
                                {
                                    Console.WriteLine($"❌ Lỗi khi load Lab: {labEx.Message}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("❌ RegistrationSchedule là null sau khi load");
                            }
                            
                            Console.WriteLine("=== END DEBUG ===\n");
                        }
                        catch (Exception reportEx)
                        {
                            Console.WriteLine($"❌ Lỗi tổng thể với ReportId {report.ReportId}: {reportEx.Message}");
                            Console.WriteLine($"Stack trace: {reportEx.StackTrace}");
                            Console.WriteLine("=== END DEBUG ===\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"ReportId {report.ReportId} có RegistrationScheduleId = null");
                    }
                }

                return reports;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi tổng thể method: {ex.Message}");
                throw ex;
            }
        }
        
        // Method helper để load RegistrationSchedule an toàn sau khi debug
        public async Task LoadRegistrationScheduleSafelyAsync(Report report)
        {
            if (report.RegistrationScheduleId.HasValue)
            {
                // Chỉ load RegistrationSchedule, không load các navigation properties bên trong
                await _DBContext.Entry(report).Reference(r => r.RegistrationSchedule).LoadAsync();
            }
        }
    }
} 