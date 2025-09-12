using BusinessLayer.ResponseModel.Analytics;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.Service;
using DataLayer.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IClassRepository _classRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IReportRepository _reportRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly ILogger<AnalyticsService> _logger;

        public AnalyticsService(
            IScheduleRepository scheduleRepository,
            IClassRepository classRepository,
            IUserRepository userRepository,
            IEquipmentRepository equipmentRepository,
            IReportRepository reportRepository,
            ISemesterRepository semesterRepository,
            ILogger<AnalyticsService> logger)
        {
            _scheduleRepository = scheduleRepository;
            _classRepository = classRepository;
            _userRepository = userRepository;
            _equipmentRepository = equipmentRepository;
            _reportRepository = reportRepository;
            _semesterRepository = semesterRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<LabUsageReportModel>> GetLabUsageReportAsync(DateTime startDate, DateTime endDate, int? subjectId = null)
        {
            try
            {
                // Lấy tất cả schedule trong khoảng thời gian
                var schedules = (await _scheduleRepository.GetByDateRangeAsync(startDate, endDate)).ToList();
                
                // Filter theo subject nếu có
                if (subjectId.HasValue)
                {
                    schedules = schedules.Where(s => s.Class?.SubjectId == subjectId.Value).ToList();
                }

                // Tính toán metrics
                var report = new LabUsageReportModel
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    TotalSessions = schedules.Count,
                    TotalClasses = schedules.Select(s => s.ClassId).Distinct().Count(),
                    TotalSubjects = schedules.Where(s => s.Class?.Subject != null)
                                          .Select(s => s.Class.SubjectId).Distinct().Count()
                };

                // Subject Usage Analysis
                var subjectGroups = schedules.Where(s => s.Class?.Subject != null)
                                            .GroupBy(s => new { s.Class.SubjectId, s.Class.Subject.SubjectName })
                                            .ToList();

                foreach (var group in subjectGroups)
                {
                    var lecturers = group.Where(s => s.Class?.Lecturer != null)
                                        .Select(s => s.Class.Lecturer.UserFullName)
                                        .Distinct().ToList();

                    report.SubjectUsage.Add(new SubjectUsageModel
                    {
                        SubjectId = group.Key.SubjectId,
                        SubjectName = group.Key.SubjectName ?? "Unknown",
                        SessionCount = group.Count(),
                        ClassCount = group.Select(s => s.ClassId).Distinct().Count(),
                        UsagePercentage = report.TotalSessions > 0 ? (double)group.Count() / report.TotalSessions * 100 : 0,
                        LecturerNames = lecturers
                    });
                }

                // Slot Usage Analysis
                var slotGroups = schedules.Where(s => s.Class?.ScheduleType?.Slot != null)
                                        .GroupBy(s => new { 
                                            s.Class.ScheduleType.Slot.SlotId, 
                                            s.Class.ScheduleType.Slot.SlotName,
                                            s.Class.ScheduleType.Slot.SlotStartTime,
                                            s.Class.ScheduleType.Slot.SlotEndTime
                                        })
                                        .ToList();

                foreach (var group in slotGroups)
                {
                    var subjects = group.Where(s => s.Class?.Subject != null)
                                       .Select(s => s.Class.Subject.SubjectName)
                                       .GroupBy(name => name)
                                       .OrderByDescending(g => g.Count())
                                       .Take(3)
                                       .Select(g => g.Key)
                                       .ToList();

                    report.SlotUsage.Add(new SlotUsageModel
                    {
                        SlotId = group.Key.SlotId,
                        SlotName = group.Key.SlotName ?? "Unknown",
                        StartTime = TimeSpan.TryParse(group.Key.SlotStartTime, out var startTime) ? startTime : TimeSpan.Zero,
                        EndTime = TimeSpan.TryParse(group.Key.SlotEndTime, out var endTime) ? endTime : TimeSpan.Zero,
                        SessionCount = group.Count(),
                        UsagePercentage = report.TotalSessions > 0 ? (double)group.Count() / report.TotalSessions * 100 : 0,
                        PopularSubjects = subjects
                    });
                }

                // Lecturer Usage Analysis
                var lecturerGroups = schedules.Where(s => s.Class?.Lecturer != null)
                                             .GroupBy(s => new { 
                                                 s.Class.LecturerId, 
                                                 s.Class.Lecturer.UserFullName,
                                                 s.Class.Lecturer.UserEmail
                                             })
                                             .ToList();

                foreach (var group in lecturerGroups)
                {
                    var subjects = group.Where(s => s.Class?.Subject != null)
                                       .Select(s => s.Class.Subject.SubjectName)
                                       .Distinct().ToList();

                    var activityScore = group.Count() * 10 + group.Select(s => s.ClassId).Distinct().Count() * 5;

                    report.LecturerUsage.Add(new LecturerUsageModel
                    {
                        LecturerId = group.Key.LecturerId ?? Guid.Empty,
                        LecturerName = group.Key.UserFullName ?? "Unknown",
                        LecturerEmail = group.Key.UserEmail ?? "Unknown",
                        SessionCount = group.Count(),
                        ClassCount = group.Select(s => s.ClassId).Distinct().Count(),
                        SubjectsTaught = subjects,
                        ActivityScore = activityScore
                    });
                }

                // Daily Usage Analysis
                var dailyGroups = schedules.GroupBy(s => s.ScheduleDate.Date).ToList();

                foreach (var group in dailyGroups)
                {
                    var subjects = group.Where(s => s.Class?.Subject != null)
                                       .Select(s => s.Class.Subject.SubjectName)
                                       .Distinct().ToList();

                    var slots = group.Where(s => s.Class?.ScheduleType?.Slot != null)
                                    .Select(s => s.Class.ScheduleType.Slot.SlotName)
                                    .Distinct().ToList();

                    report.DailyUsage.Add(new DailyUsageModel
                    {
                        Date = group.Key,
                        SessionCount = group.Count(),
                        SubjectsOnDay = subjects,
                        SlotsUsed = slots
                    });
                }

                // Sort results
                report.SubjectUsage = report.SubjectUsage.OrderByDescending(s => s.SessionCount).ToList();
                report.SlotUsage = report.SlotUsage.OrderByDescending(s => s.SessionCount).ToList();
                report.LecturerUsage = report.LecturerUsage.OrderByDescending(l => l.ActivityScore).ToList();
                report.DailyUsage = report.DailyUsage.OrderBy(d => d.Date).ToList();

                return new BaseResponse<LabUsageReportModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy báo cáo sử dụng lab thành công!",
                    Data = report
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLabUsageReportAsync: {Message}", ex.Message);
                return new BaseResponse<LabUsageReportModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<LabUsageReportModel>> GetLabUsageMonthlyAsync(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            
            return await GetLabUsageReportAsync(startDate, endDate);
        }

        public async Task<BaseResponse<List<LabUsageDetailModel>>> GetLabUsageDetailAsync(DateTime startDate, DateTime endDate, int? subjectId = null, Guid? lecturerId = null)
        {
            try
            {
                var schedules = (await _scheduleRepository.GetByDateRangeAsync(startDate, endDate)).ToList();
                
                // Apply filters
                if (subjectId.HasValue)
                {
                    schedules = schedules.Where(s => s.Class?.SubjectId == subjectId.Value).ToList();
                }
                
                if (lecturerId.HasValue)
                {
                    schedules = schedules.Where(s => s.Class?.LecturerId == lecturerId.Value).ToList();
                }

                var details = schedules.Select(s => new LabUsageDetailModel
                {
                    ScheduleId = s.ScheduleId,
                    ScheduleName = s.ScheduleName ?? "Unknown",
                    ScheduleDate = s.ScheduleDate,
                    ClassName = s.Class?.ClassName ?? "Unknown",
                    SubjectName = s.Class?.Subject?.SubjectName ?? "Unknown",
                    LecturerName = s.Class?.Lecturer?.UserFullName ?? "Unknown",
                    SlotName = s.Class?.ScheduleType?.Slot?.SlotName ?? "Unknown",
                    StartTime = TimeSpan.TryParse(s.Class?.ScheduleType?.Slot?.SlotStartTime, out var startTime) ? startTime : TimeSpan.Zero,
                    EndTime = TimeSpan.TryParse(s.Class?.ScheduleType?.Slot?.SlotEndTime, out var endTime) ? endTime : TimeSpan.Zero,
                    Status = "Active" // Default status since Schedule entity doesn't have status field
                }).OrderBy(d => d.ScheduleDate).ThenBy(d => d.StartTime).ToList();

                return new BaseResponse<List<LabUsageDetailModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết sử dụng lab thành công!",
                    Data = details
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLabUsageDetailAsync: {Message}", ex.Message);
                return new BaseResponse<List<LabUsageDetailModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<LabUsageReportModel>> GetLabUsageBySemesterAsync(int semesterId)
        {
            try
            {
                // Lấy thông tin semester từ database
                var semester = await _semesterRepository.GetByIdAsync(semesterId);
                if (semester == null)
                {
                    return new BaseResponse<LabUsageReportModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy học kỳ với ID đã cung cấp!",
                        Data = null
                    };
                }

                // Sử dụng ngày bắt đầu và kết thúc từ semester entity
                var startDate = semester.SemesterStartDate;
                var endDate = semester.SemesterEndDate;
                
                _logger.LogInformation("Getting lab usage report for semester {SemesterName} from {StartDate} to {EndDate}", 
                    semester.SemesterName, startDate.ToString("dd/MM/yyyy"), endDate.ToString("dd/MM/yyyy"));
                
                var result = await GetLabUsageReportAsync(startDate, endDate);
                
                // Cập nhật message để bao gồm thông tin semester
                if (result.Success && result.Data != null)
                {
                    result.Message = $"Lấy báo cáo sử dụng lab cho học kỳ {semester.SemesterName} thành công!";
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLabUsageBySemesterAsync for semesterId {SemesterId}: {Message}", semesterId, ex.Message);
                return new BaseResponse<LabUsageReportModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi khi lấy báo cáo theo học kỳ: {ex.Message}",
                    Data = null
                };
            }
        }

        /// <summary>
        /// Lấy thống kê tổng quan hệ thống - Tổng số User, Equipment và Report
        /// </summary>
        /// <returns>Thống kê tổng quan hệ thống với mô tả tiếng Việt</returns>
        public async Task<BaseResponse<SystemOverviewModel>> GetSystemOverviewAsync()
        {
            try
            {
                _logger.LogInformation("Bắt đầu lấy thống kê tổng quan hệ thống");

                // Lấy tất cả dữ liệu cần thiết
                var allUsers = await _userRepository.GetAllUser();
                var allEquipments = await _equipmentRepository.GetAllEquipment();
                var allReports = await _reportRepository.GetAllAsync();

                var usersList = allUsers.ToList();
                var equipmentsList = allEquipments.ToList();
                var reportsList = allReports.ToList();

                // Tạo model thống kê tổng quan
                var overview = new SystemOverviewModel
                {
                    TongSoNguoiDung = usersList.Count,
                    TongSoThietBi = equipmentsList.Count,
                    TongSoBaoCao = reportsList.Count,
                    ThoiGianCapNhat = DateTime.Now
                };

                // Thống kê người dùng theo vai trò
                var userRoleGroups = usersList.GroupBy(u => u.UserRoleName ?? "Unknown").ToList();
                foreach (var group in userRoleGroups)
                {
                    var roleName = group.Key;
                    var roleDescription = GetRoleDescription(roleName);
                    var count = group.Count();
                    var percentage = overview.TongSoNguoiDung > 0 ? (double)count / overview.TongSoNguoiDung * 100 : 0;

                    overview.ThongKeNguoiDungTheoVaiTro.Add(new UserRoleStatsModel
                    {
                        TenVaiTro = roleName,
                        SoLuong = count,
                        PhanTram = Math.Round(percentage, 2),
                        MoTaVaiTro = roleDescription
                    });
                }

                // Thống kê thiết bị theo loại
                var equipmentTypeGroups = equipmentsList
                    .Where(e => e.EquipmentType != null)
                    .GroupBy(e => new { e.EquipmentType.EquipmentTypeId, e.EquipmentType.EquipmentTypeName })
                    .ToList();

                foreach (var group in equipmentTypeGroups)
                {
                    var count = group.Count();
                    var percentage = overview.TongSoThietBi > 0 ? (double)count / overview.TongSoThietBi * 100 : 0;
                    
                    // Đếm thiết bị theo trạng thái (chỉ có ACTIVE và INACTIVE)
                    var workingCount = group.Count(e => e.EquipmentStatus == "Available");
                    var brokenCount = group.Count(e => e.EquipmentStatus == "INACTIVE");

                    overview.ThongKeThietBiTheoLoai.Add(new EquipmentTypeStatsModel
                    {
                        LoaiThietBiId = group.Key.EquipmentTypeId,
                        TenLoaiThietBi = group.Key.EquipmentTypeName ?? "Không xác định",
                        SoLuong = count,
                        PhanTram = Math.Round(percentage, 2),
                        SoLuongHoatDong = workingCount,
                        SoLuongKhongHoatDong = brokenCount
                    });
                }

                // Thống kê báo cáo theo trạng thái
                var reportStatusGroups = reportsList.GroupBy(r => r.ReportStatus ?? "Unknown").ToList();
                foreach (var group in reportStatusGroups)
                {
                    var status = group.Key;
                    var statusDescription = GetReportStatusDescription(status);
                    var count = group.Count();
                    var percentage = overview.TongSoBaoCao > 0 ? (double)count / overview.TongSoBaoCao * 100 : 0;

                    overview.ThongKeBaoCaoTheoTrangThai.Add(new ReportStatusStatsModel
                    {
                        TrangThai = status,
                        SoLuong = count,
                        PhanTram = Math.Round(percentage, 2),
                        MoTaTrangThai = statusDescription
                    });
                }

                // Sắp xếp kết quả theo số lượng giảm dần
                overview.ThongKeNguoiDungTheoVaiTro = overview.ThongKeNguoiDungTheoVaiTro
                    .OrderByDescending(x => x.SoLuong).ToList();
                overview.ThongKeThietBiTheoLoai = overview.ThongKeThietBiTheoLoai
                    .OrderByDescending(x => x.SoLuong).ToList();
                overview.ThongKeBaoCaoTheoTrangThai = overview.ThongKeBaoCaoTheoTrangThai
                    .OrderByDescending(x => x.SoLuong).ToList();

                _logger.LogInformation("Lấy thống kê tổng quan thành công: {UserCount} người dùng, {EquipmentCount} thiết bị, {ReportCount} báo cáo", 
                    overview.TongSoNguoiDung, overview.TongSoThietBi, overview.TongSoBaoCao);

                return new BaseResponse<SystemOverviewModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thống kê tổng quan hệ thống thành công!",
                    Data = overview
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thống kê tổng quan hệ thống: {Message}", ex.Message);
                return new BaseResponse<SystemOverviewModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi hệ thống: {ex.Message}",
                    Data = null
                };
            }
        }

        /// <summary>
        /// Lấy mô tả vai trò bằng tiếng Việt
        /// </summary>
        private string GetRoleDescription(string role)
        {
            return role switch
            {
                "Admin" => "Quản trị viên hệ thống",
                "HeadOfDepartment" => "Trưởng khoa/Trưởng bộ môn",
                "Lecturer" => "Giảng viên",
                "Student" => "Sinh viên",
                _ => "Vai trò không xác định"
            };
        }

        /// <summary>
        /// Lấy mô tả trạng thái báo cáo bằng tiếng Việt
        /// </summary>
        private string GetReportStatusDescription(string status)
        {
            return status switch
            {
                "Pending" => "Đang chờ xử lý",
                "InProgress" => "Đang xử lý",
                "Completed" => "Đã hoàn thành",
                "Approved" => "Đã phê duyệt",
                "Rejected" => "Đã từ chối",
                "Draft" => "Bản nháp",
                "Submitted" => "Đã nộp",
                "Graded" => "Đã chấm điểm",
                _ => "Trạng thái không xác định"
            };
        }

    }
}
