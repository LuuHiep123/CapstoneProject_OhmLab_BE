using BusinessLayer.RequestModel.Report;
using BusinessLayer.ResponseModel.Report;
using BusinessLayer.ResponseModel.BaseResponse;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Linq;
using BusinessLayer.ResponseModel.Slot;
using BusinessLayer.ResponseModel.Class;

namespace BusinessLayer.Service.Implement
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IUserRepositoty _userRepository;
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportService> _logger;

        public ReportService(
            IReportRepository reportRepository,
            IScheduleRepository scheduleRepository,
            IUserRepositoty userRepository,
            IClassRepository classRepository,
            ISubjectRepository subjectRepository,
            IMapper mapper,
            ILogger<ReportService> logger)
        {
            _reportRepository = reportRepository;
            _scheduleRepository = scheduleRepository;
            _userRepository = userRepository;
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ReportResponseModel>> CreateReportAsync(CreateReportRequestModel model, Guid userId)
        {
            try
            {
                // Validate user exists
                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return new BaseResponse<ReportResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy người dùng!",
                        Data = null
                    };
                }

                // Find schedule based on today's date, slot, and class
                var today = DateTime.Today;
                var schedule = await FindScheduleByUserSelectionAsync(userId, today, model.SelectedSlot, model.SelectedClass);
                if (schedule == null)
                {
                    return new BaseResponse<ReportResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lịch học phù hợp cho hôm nay!",
                        Data = null
                    };
                }

                var report = new Report
                {
                    UserId = userId,
                    ScheduleId = schedule.ScheduleId,
                    ReportTitle = model.ReportTitle,
                    ReportDescription = model.ReportDescription,
                    ReportCreateDate = DateTime.Now,
                    ReportStatus = "Pending"
                };

                var createdReport = await _reportRepository.CreateAsync(report);
                var response = await MapToReportResponseModel(createdReport);

                return new BaseResponse<ReportResponseModel>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo báo cáo thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateReport: {Message}", ex.Message);
                return new BaseResponse<ReportResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi hệ thống!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<object>> GetTodaySlotsAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return new BaseResponse<object>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy người dùng!",
                        Data = null
                    };
                }

                var today = DateTime.Today;
                var schedules = await GetUserSchedulesAsync(userId);
                var todaySchedules = schedules.Where(s => s.ScheduleDate.Date == today).ToList();

                var availableSlots = todaySchedules
                    .GroupBy(s => s.Class?.ScheduleType?.Slot?.SlotName ?? "Unknown")
                    .Select(g => new BusinessLayer.ResponseModel.Report.SlotResponseModel
                    {
                        SlotName = g.Key,
                        SlotStartTime = g.First().Class?.ScheduleType?.Slot?.SlotStartTime ?? "Unknown",
                        SlotEndTime = g.First().Class?.ScheduleType?.Slot?.SlotEndTime ?? "Unknown",
                        ScheduleCount = g.Count()
                    })
                    .OrderBy(s => s.SlotStartTime)
                    .ToList();

                var result = new
                {
                    Slots = availableSlots,
                    TotalCount = availableSlots.Count,
                    Today = today.ToString("dd/MM/yyyy")
                };

                return new BaseResponse<object>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách slot hôm nay thành công!",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTodaySlots: {Message}", ex.Message);
                return new BaseResponse<object>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi hệ thống!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<object>> GetTodayClassesAsync(Guid userId, string slotName)
        {
            try
            {
                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return new BaseResponse<object>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy người dùng!",
                        Data = null
                    };
                }

                var today = DateTime.Today;
                var schedules = await GetUserSchedulesAsync(userId);
                var filteredSchedules = schedules
                    .Where(s => s.ScheduleDate.Date == today && 
                               s.Class?.ScheduleType?.Slot?.SlotName == slotName)
                    .ToList();

                var availableClasses = filteredSchedules
                    .Select(s => new BusinessLayer.ResponseModel.Report.ClassResponseModel
                    {
                        ClassName = s.Class?.ClassName ?? "Unknown",
                        SubjectName = s.Class?.Subject?.SubjectName ?? "Unknown",
                        LecturerName = s.Class?.Lecturer?.UserFullName ?? "Unknown",
                        ScheduleId = s.ScheduleId
                    })
                    .OrderBy(c => c.ClassName)
                    .ToList();

                var result = new
                {
                    Classes = availableClasses,
                    TotalCount = availableClasses.Count,
                    Today = today.ToString("dd/MM/yyyy"),
                    SlotName = slotName
                };

                return new BaseResponse<object>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách lớp hôm nay thành công!",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTodayClasses: {Message}", ex.Message);
                return new BaseResponse<object>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi hệ thống!",
                    Data = null
                };
            }
        }

        // Helper methods
        private async Task<List<Schedule>> GetUserSchedulesAsync(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return new List<Schedule>();

            var allSchedules = await _scheduleRepository.GetAllAsync();
            var userSchedules = new List<Schedule>();

            if (user.UserRoleName == "Student")
            {
                var classUsers = await _classRepository.GetAllAsync();
                var studentClasses = classUsers.Where(c => c.ClassUsers.Any(cu => cu.UserId == userId));
                var classIds = studentClasses.Select(c => c.ClassId).ToList();
                userSchedules = allSchedules.Where(s => classIds.Contains(s.ClassId)).ToList();
            }
            else if (user.UserRoleName == "Lecturer")
            {
                var lecturerClasses = await _classRepository.GetByLecturerIdAsync(userId);
                var classIds = lecturerClasses.Select(c => c.ClassId).ToList();
                userSchedules = allSchedules.Where(s => classIds.Contains(s.ClassId)).ToList();
            }
            else
            {
                // Admin/HeadOfDepartment can see all schedules
                userSchedules = allSchedules.ToList();
            }

            return userSchedules;
        }

        private async Task<Schedule?> FindScheduleByUserSelectionAsync(Guid userId, DateTime date, string slotName, string className)
        {
            var schedules = await GetUserSchedulesAsync(userId);
            return schedules.FirstOrDefault(s => 
                s.ScheduleDate.Date == date.Date &&
                s.Class?.ScheduleType?.Slot?.SlotName == slotName &&
                s.Class?.ClassName == className);
        }

        // Existing methods (simplified)
        public async Task<BaseResponse<ReportResponseModel>> GetReportByIdAsync(int reportId)
        {
            try
            {
                var report = await _reportRepository.GetByIdAsync(reportId);
                if (report == null)
                {
                    return new BaseResponse<ReportResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy báo cáo!",
                        Data = null
                    };
                }

                var response = await MapToReportResponseModel(report);
                return new BaseResponse<ReportResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin báo cáo thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportById: {Message}", ex.Message);
                return new BaseResponse<ReportResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi hệ thống!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ReportDetailResponseModel>> GetReportDetailAsync(int reportId)
        {
            try
            {
                var report = await _reportRepository.GetByIdAsync(reportId);
                if (report == null)
                {
                    return new BaseResponse<ReportDetailResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy báo cáo!",
                        Data = null
                    };
                }

                var response = await MapToReportDetailResponseModel(report);
                return new BaseResponse<ReportDetailResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin chi tiết báo cáo thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportDetail: {Message}", ex.Message);
                return new BaseResponse<ReportDetailResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi hệ thống!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<object>> GetAllReportsAsync()
        {
            try
            {
                var reports = await _reportRepository.GetAllAsync();
                
                var reportResponses = new List<ReportResponseModel>();
                foreach (var report in reports)
                {
                    var response = await MapToReportResponseModel(report);
                    reportResponses.Add(response);
                }

                var result = new
                {
                    Reports = reportResponses,
                    TotalCount = reportResponses.Count
                };

                return new BaseResponse<object>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách báo cáo thành công!",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllReports: {Message}", ex.Message);
                return new BaseResponse<object>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi hệ thống!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ReportResponseModel>> UpdateReportStatusAsync(int reportId, UpdateReportStatusRequestModel model)
        {
            try
            {
                var report = await _reportRepository.GetByIdAsync(reportId);
                if (report == null)
                {
                    return new BaseResponse<ReportResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy báo cáo!",
                        Data = null
                    };
                }

                report.ReportStatus = model.ReportStatus;
                if (!string.IsNullOrEmpty(model.ResolutionNotes))
                {
                    report.ReportDescription = report.ReportDescription + "\n\n--- GHI CHÚ GIẢI QUYẾT ---\n" + model.ResolutionNotes;
                }

                var updatedReport = await _reportRepository.UpdateAsync(report);
                var response = await MapToReportResponseModel(updatedReport);

                return new BaseResponse<ReportResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật trạng thái báo cáo thành công!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateReportStatus: {Message}", ex.Message);
                return new BaseResponse<ReportResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi hệ thống!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ReportStatisticsResponseModel>> GetReportStatisticsAsync()
        {
            try
            {
                var reports = await _reportRepository.GetAllAsync();
                
                var statistics = new ReportStatisticsResponseModel
                {
                    TotalReports = reports.Count(),
                    PendingReports = reports.Count(r => r.ReportStatus == "Pending"),
                    InProgressReports = reports.Count(r => r.ReportStatus == "In Progress"),
                    ResolvedReports = reports.Count(r => r.ReportStatus == "Resolved"),
                    ClosedReports = reports.Count(r => r.ReportStatus == "Closed")
                };

                var statusGroups = reports.GroupBy(r => r.ReportStatus)
                    .Select(g => new ReportByStatusModel
                    {
                        Status = g.Key,
                        Count = g.Count(),
                        Percentage = (double)g.Count() / reports.Count() * 100
                    }).ToList();
                statistics.ReportsByStatus = statusGroups;

                var monthGroups = reports.GroupBy(r => new { r.ReportCreateDate.Year, r.ReportCreateDate.Month })
                    .Select(g => new ReportByMonthModel
                    {
                        Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                        Count = g.Count()
                    }).OrderBy(x => x.Month).ToList();
                statistics.ReportsByMonth = monthGroups;

                return new BaseResponse<ReportStatisticsResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thống kê báo cáo thành công!",
                    Data = statistics
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportStatistics: {Message}", ex.Message);
                return new BaseResponse<ReportStatisticsResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi hệ thống!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<object>> GetReportsByUserAsync(Guid userId)
        {
            try
            {
                var reports = await _reportRepository.GetByUserIdAsync(userId);
                
                var reportResponses = new List<ReportResponseModel>();
                foreach (var report in reports)
                {
                    var response = await MapToReportResponseModel(report);
                    reportResponses.Add(response);
                }

                var result = new
                {
                    Reports = reportResponses,
                    TotalCount = reportResponses.Count
                };

                return new BaseResponse<object>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách báo cáo của người dùng thành công!",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportsByUser: {Message}", ex.Message);
                return new BaseResponse<object>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi hệ thống!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<object>> GetReportsByScheduleAsync(int scheduleId)
        {
            try
            {
                var reports = await _reportRepository.GetByScheduleIdAsync(scheduleId);
                
                var reportResponses = new List<ReportResponseModel>();
                foreach (var report in reports)
                {
                    var response = await MapToReportResponseModel(report);
                    reportResponses.Add(response);
                }

                var result = new
                {
                    Reports = reportResponses,
                    TotalCount = reportResponses.Count
                };

                return new BaseResponse<object>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách báo cáo theo lịch học thành công!",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportsBySchedule: {Message}", ex.Message);
                return new BaseResponse<object>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi hệ thống!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<object>> GetPendingIncidentsAsync()
        {
            try
            {
                var reports = await _reportRepository.GetAllAsync();
                var pendingReports = reports.Where(r => 
                    r.ReportStatus == "Pending" &&
                    (r.ReportTitle.Contains("Chập mạch") ||
                     r.ReportTitle.Contains("Thiết bị hỏng") ||
                     r.ReportTitle.Contains("Tai nạn") ||
                     r.ReportTitle.Contains("Sự cố") ||
                     r.ReportTitle.Contains("Hỏng") ||
                     r.ReportTitle.Contains("Lỗi"))
                );
                
                var reportResponses = new List<ReportResponseModel>();
                foreach (var report in pendingReports)
                {
                    var response = await MapToReportResponseModel(report);
                    reportResponses.Add(response);
                }

                var result = new
                {
                    Incidents = reportResponses,
                    TotalCount = reportResponses.Count
                };

                return new BaseResponse<object>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách sự cố chờ xử lý thành công!",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPendingIncidents: {Message}", ex.Message);
                return new BaseResponse<object>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi hệ thống!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<object>> GetResolvedIncidentsAsync()
        {
            try
            {
                var reports = await _reportRepository.GetAllAsync();
                var resolvedReports = reports.Where(r => 
                    (r.ReportStatus == "Resolved" || r.ReportStatus == "Closed") &&
                    (r.ReportTitle.Contains("Chập mạch") ||
                     r.ReportTitle.Contains("Thiết bị hỏng") ||
                     r.ReportTitle.Contains("Tai nạn") ||
                     r.ReportTitle.Contains("Sự cố") ||
                     r.ReportTitle.Contains("Hỏng") ||
                     r.ReportTitle.Contains("Lỗi"))
                );
                
                var reportResponses = new List<ReportResponseModel>();
                foreach (var report in resolvedReports)
                {
                    var response = await MapToReportResponseModel(report);
                    reportResponses.Add(response);
                }

                var result = new
                {
                    Incidents = reportResponses,
                    TotalCount = reportResponses.Count
                };

                return new BaseResponse<object>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách sự cố đã giải quyết thành công!",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetResolvedIncidents: {Message}", ex.Message);
                return new BaseResponse<object>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi hệ thống!",
                    Data = null
                };
            }
        }

        private async Task<ReportResponseModel> MapToReportResponseModel(Report report)
        {
            var user = await _userRepository.GetUserById(report.UserId);
            var schedule = await _scheduleRepository.GetByIdAsync(report.ScheduleId);

            return new ReportResponseModel
            {
                ReportId = report.ReportId,
                UserId = report.UserId,
                UserName = user?.UserFullName ?? "Unknown",
                ScheduleId = report.ScheduleId,
                ScheduleName = schedule?.ScheduleName ?? "Unknown",
                ReportTitle = report.ReportTitle,
                ReportDescription = report.ReportDescription,
                ReportCreateDate = report.ReportCreateDate,
                ReportStatus = report.ReportStatus,
                ResolutionNotes = report.ReportDescription?.Contains("--- GHI CHÚ GIẢI QUYẾT ---") == true 
                    ? report.ReportDescription.Split("--- GHI CHÚ GIẢI QUYẾT ---").LastOrDefault()?.Trim()
                    : null
            };
        }

        private async Task<ReportDetailResponseModel> MapToReportDetailResponseModel(Report report)
        {
            var user = await _userRepository.GetUserById(report.UserId);
            var schedule = await _scheduleRepository.GetByIdAsync(report.ScheduleId);
            var classEntity = schedule != null ? await _classRepository.GetByIdAsync(schedule.ClassId) : null;
            var subject = classEntity != null ? await _subjectRepository.GetSubjectById(classEntity.SubjectId) : null;
            
            string slotName = "Unknown";
            string slotStartTime = "Unknown";
            string slotEndTime = "Unknown";
            
            if (classEntity?.ScheduleType != null)
            {
                slotName = classEntity.ScheduleType.Slot?.SlotName ?? "Unknown";
                slotStartTime = classEntity.ScheduleType.Slot?.SlotStartTime ?? "Unknown";
                slotEndTime = classEntity.ScheduleType.Slot?.SlotEndTime ?? "Unknown";
            }

            return new ReportDetailResponseModel
            {
                ReportId = report.ReportId,
                UserId = report.UserId,
                UserName = user?.UserFullName ?? "Unknown",
                ScheduleId = report.ScheduleId,
                ScheduleName = schedule?.ScheduleName ?? "Unknown",
                ReportTitle = report.ReportTitle,
                ReportDescription = report.ReportDescription,
                ReportCreateDate = report.ReportCreateDate,
                ReportStatus = report.ReportStatus,
                ResolutionNotes = report.ReportDescription?.Contains("--- GHI CHÚ GIẢI QUYẾT ---") == true 
                    ? report.ReportDescription.Split("--- GHI CHÚ GIẢI QUYẾT ---").LastOrDefault()?.Trim()
                    : null,
                ClassName = classEntity?.ClassName ?? "Unknown",
                SubjectName = subject?.SubjectName ?? "Unknown",
                LecturerName = classEntity?.Lecturer?.UserFullName ?? "Unknown",
                ScheduleDate = schedule?.ScheduleDate ?? DateTime.MinValue,
                SlotName = slotName,
                SlotStartTime = slotStartTime,
                SlotEndTime = slotEndTime
            };
        }
    }
} 