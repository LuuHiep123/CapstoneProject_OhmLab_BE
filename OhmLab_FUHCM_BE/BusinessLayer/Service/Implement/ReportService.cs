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
        private readonly IUserRepository _userRepository;
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IRegistrationScheduleRepository _registrationScheduleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportService> _logger;

        public ReportService(
            IReportRepository reportRepository,
            IScheduleRepository scheduleRepository,
            IUserRepository userRepository,
            IClassRepository classRepository,
            ISubjectRepository subjectRepository,
            IRegistrationScheduleRepository registrationScheduleRepository,
            IMapper mapper,
            ILogger<ReportService> logger)
        {
            _reportRepository = reportRepository;
            _scheduleRepository = scheduleRepository;
            _userRepository = userRepository;
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _registrationScheduleRepository = registrationScheduleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ReportResponseModel>> CreateReportAsync(CreateReportRequestModel model, Guid userId)
        {
            try
            {
                // Validate user exists and has proper role
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

                // Only Student and Lecturer can create reports
                if (user.UserRoleName != "Student" && user.UserRoleName != "Lecturer")
                {
                    return new BaseResponse<ReportResponseModel>
                    {
                        Code = 403,
                        Success = false,
                        Message = "Chỉ sinh viên và giảng viên mới có thể tạo báo cáo!",
                        Data = null
                    };
                }

                // Find schedule based on today's date, slot, and class
                var today = DateTime.Today;
                var schedule = await FindScheduleByUserSelectionAsync(userId, today, model.SelectedSlot, model.SelectedClass, user.UserRoleName);
                if (schedule == null)
                {
                    return new BaseResponse<ReportResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lịch học phù hợp cho hôm nay hoặc bạn không có quyền truy cập!",
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
                    Message = "Co loi con vo oi",
                    Data = null
                };
            }
        }
            public async Task<BaseResponse<object>> GetTodaySlotsAsync(Guid userId)
            {
                try
                {
                    // Validate user
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

                    // Validate user role
                    if (user.UserRoleName != "Student" && user.UserRoleName != "Lecturer")
                    {
                        return new BaseResponse<object>
                        {
                            Code = 403,
                            Success = false,
                            Message = "Chỉ sinh viên và giảng viên mới có thể xem lịch học!",
                            Data = null
                        };
                    }

                    var today = DateTime.Today;

                    // Get user's schedules for today with proper navigation properties
                    var todaySchedules = await GetUserTodaySchedulesWithIncludesAsync(userId, user.UserRoleName);

                    if (!todaySchedules.Any())
                    {
                        _logger.LogInformation("No schedules found for user {UserId} on {Date}", userId, today.ToString("yyyy-MM-dd"));

                        var emptyResult = new
                        {
                            Slots = new List<BusinessLayer.ResponseModel.Report.SlotResponseModel>(),
                            TotalCount = 0,
                            Today = today.ToString("dd/MM/yyyy"),
                            Message = "Không có lịch học hôm nay"
                        };

                        return new BaseResponse<object>
                        {
                            Code = 200,
                            Success = true,
                            Message = "Không có lịch học hôm nay!",
                            Data = emptyResult
                        };
                    }

                    // Group by slot information from RegistrationSchedule
                    var availableSlots = todaySchedules
                        .Where(s => s.Slot != null)
                        .GroupBy(s => new {
                            SlotId = s.Slot.SlotId,
                            SlotName = s.Slot.SlotName,
                            SlotStartTime = s.Slot.SlotStartTime,
                            SlotEndTime = s.Slot.SlotEndTime
                        })
                        .Select(g => new BusinessLayer.ResponseModel.Report.SlotResponseModel
                        {
                            SlotName = g.Key.SlotName,
                            SlotStartTime = g.Key.SlotStartTime,
                            SlotEndTime = g.Key.SlotEndTime,
                            ScheduleCount = g.Count()
                        })
                        .OrderBy(s => s.SlotStartTime)
                        .ToList();

                    _logger.LogInformation("Found {SlotCount} slots for user {UserId} on {Date}", availableSlots.Count, userId, today.ToString("yyyy-MM-dd"));

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
                    _logger.LogError(ex, "Error in GetTodaySlots for user {UserId}: {Message}", userId, ex.Message);
                    return new BaseResponse<object>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Lỗi hệ thống khi lấy danh sách slot!",
                        Data = null
                    };
                }
            }

            public async Task<BaseResponse<object>> GetTodayClassesAsync(Guid userId, string slotName)
            {
                try
                {
                    // Validate user
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

                    // Validate user role
                    if (user.UserRoleName != "Student" && user.UserRoleName != "Lecturer")
                    {
                        return new BaseResponse<object>
                        {
                            Code = 403,
                            Success = false,
                            Message = "Chỉ sinh viên và giảng viên mới có thể xem lịch học!",
                            Data = null
                        };
                    }

                    // Validate slot name parameter
                    if (string.IsNullOrWhiteSpace(slotName))
                    {
                        return new BaseResponse<object>
                        {
                            Code = 400,
                            Success = false,
                            Message = "Tên slot không được để trống!",
                            Data = null
                        };
                    }

                    var today = DateTime.Today;

                    // Get user's schedules for today with proper navigation properties
                    var todaySchedules = await GetUserTodaySchedulesWithIncludesAsync(userId, user.UserRoleName);

                    // Filter schedules by slot name and ensure complete navigation properties
                    var filteredSchedules = todaySchedules
                        .Where(s => s.Slot?.SlotName == slotName)
                        .ToList();

                    if (!filteredSchedules.Any())
                    {
                        _logger.LogInformation("No schedules found for user {UserId} with slot '{SlotName}' on {Date}", userId, slotName, today.ToString("yyyy-MM-dd"));

                        var emptyResult = new
                        {
                            Classes = new List<BusinessLayer.ResponseModel.Report.ClassResponseModel>(),
                            TotalCount = 0,
                            Today = today.ToString("dd/MM/yyyy"),
                            SlotName = slotName,
                            Message = $"Không có lớp học trong slot '{slotName}' hôm nay"
                        };

                        return new BaseResponse<object>
                        {
                            Code = 200,
                            Success = true,
                            Message = $"Không có lớp học trong slot '{slotName}' hôm nay!",
                            Data = emptyResult
                        };
                    }

                    // Map to response models with proper null checks
                    var availableClasses = filteredSchedules
                        .Select(s => new BusinessLayer.ResponseModel.Report.ClassResponseModel
                        {
                            ClassName = s.Class?.ClassName ?? "Unknown",
                            SubjectName = s.Class?.Subject?.SubjectName ?? "Unknown",
                            LecturerName = s.User?.UserFullName ?? "Unknown",
                            ScheduleId = s.RegistrationScheduleId
                        })
                        .OrderBy(c => c.ClassName)
                        .ToList();

                    _logger.LogInformation("Found {ClassCount} classes for user {UserId} in slot '{SlotName}' on {Date}", availableClasses.Count, userId, slotName, today.ToString("yyyy-MM-dd"));

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
                    _logger.LogError(ex, "Error in GetTodayClasses for user {UserId}, slot '{SlotName}': {Message}", userId, slotName, ex.Message);
                    return new BaseResponse<object>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Lỗi hệ thống khi lấy danh sách lớp!",
                        Data = null
                    };
                }
            }

            // Helper methods
            private async Task<List<Schedule>> GetUserTodaySchedulesAsync(Guid userId, string userRole)
            {
                try
                {
                    var today = DateTime.Today;
                    var todaySchedules = await _scheduleRepository.GetByDateAsync(today);
                    var userSchedules = new List<Schedule>();

                    _logger.LogInformation($"Found {todaySchedules.Count()} schedules for today: {today:yyyy-MM-dd}");

                    if (userRole == "Student")
                    {
                        var studentClasses = await _classRepository.GetByStudentIdAsync(userId);
                        var classIds = studentClasses.Select(c => c.ClassId).ToList();
                        userSchedules = todaySchedules.Where(s => classIds.Contains(s.ClassId)).ToList();

                        _logger.LogInformation($"Student {userId} has {studentClasses.Count} classes, {userSchedules.Count} schedules today");
                    }
                    else if (userRole == "Lecturer")
                    {
                        var lecturerClasses = await _classRepository.GetByLecturerIdAsync(userId);
                        var classIds = lecturerClasses.Select(c => c.ClassId).ToList();
                        userSchedules = todaySchedules.Where(s => classIds.Contains(s.ClassId)).ToList();

                        _logger.LogInformation($"Lecturer {userId} has {lecturerClasses.Count} classes, {userSchedules.Count} schedules today");
                    }

                    // Validate navigation properties are loaded
                    foreach (var schedule in userSchedules)
                    {
                        if (schedule.Class == null)
                        {
                            _logger.LogWarning($"Schedule {schedule.ScheduleId} has null Class");
                            continue;
                        }
                        if (schedule.Class.ScheduleType == null)
                        {
                            _logger.LogWarning($"Schedule {schedule.ScheduleId} Class {schedule.Class.ClassId} has null ScheduleType");
                            continue;
                        }
                        if (schedule.Class.ScheduleType.Slot == null)
                        {
                            _logger.LogWarning($"Schedule {schedule.ScheduleId} ScheduleType {schedule.Class.ScheduleType.ScheduleTypeId} has null Slot");
                            continue;
                        }

                        _logger.LogDebug($"Schedule {schedule.ScheduleId}: Class {schedule.Class.ClassName} -> ScheduleType {schedule.Class.ScheduleType.ScheduleTypeName} -> Slot {schedule.Class.ScheduleType.Slot.SlotName}");
                    }

                    return userSchedules;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in GetUserTodaySchedulesAsync for user {UserId}, role {UserRole}", userId, userRole);
                    return new List<Schedule>();
                }
            }

            private async Task<List<RegistrationSchedule>> GetUserTodaySchedulesWithIncludesAsync(Guid userId, string userRole)
            {
                try
                {
                    var today = DateTime.Today;
                    var userSchedules = new List<RegistrationSchedule>();

                    if (userRole == "Student")
                    {
                        // Get student's classes
                        var studentClasses = await _classRepository.GetByStudentIdAsync(userId);
                        var classIds = studentClasses.Select(c => c.ClassId).ToList();

                        // Get lab schedules for today with includes for student's classes
                        var todayLabSchedules = await _registrationScheduleRepository.GetByDateWithIncludesAsync(today);
                        userSchedules = todayLabSchedules.Where(s => classIds.Contains(s.ClassId)).ToList();

                        _logger.LogInformation("Student {UserId} has {ClassCount} classes, {LabScheduleCount} lab schedules today", userId, studentClasses.Count, userSchedules.Count);
                    }
                    else if (userRole == "Lecturer")
                    {
                        // Get lab schedules for today for this lecturer with includes
                        userSchedules = await _registrationScheduleRepository.GetByTeacherIdAndDateWithIncludesAsync(userId, today);

                        _logger.LogInformation("Lecturer {UserId} has {LabScheduleCount} lab schedules today", userId, userSchedules.Count);
                    }

                    // Log any schedules with missing navigation properties for debugging
                    var schedulesWithMissingProps = userSchedules.Where(s =>
                        s.Class == null ||
                        s.Slot == null ||
                        s.Lab == null ||
                        s.User == null).ToList();

                    if (schedulesWithMissingProps.Any())
                    {
                        _logger.LogWarning("Found {Count} lab schedules with missing navigation properties", schedulesWithMissingProps.Count);
                        foreach (var schedule in schedulesWithMissingProps)
                        {
                            _logger.LogWarning("LabSchedule {ScheduleId}: Class={ClassNull}, Slot={SlotNull}, Lab={LabNull}, User={UserNull}",
                                schedule.RegistrationScheduleId,
                                schedule.Class == null ? "Null" : "OK",
                                schedule.Slot == null ? "Null" : "OK",
                                schedule.Lab == null ? "Null" : "OK",
                                schedule.User == null ? "Null" : "OK");
                        }
                    }

                    return userSchedules;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in GetUserTodaySchedulesWithIncludesAsync for user {UserId}, role {UserRole}", userId, userRole);
                    return new List<RegistrationSchedule>();
                }
            }

            private async Task<bool> ValidateUserAccessToSchedule(Guid userId, int scheduleId, string userRole)
            {
                var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);
                if (schedule == null) return false;

                if (userRole == "Student")
                {
                    var studentClasses = await _classRepository.GetByStudentIdAsync(userId);
                    return studentClasses.Any(c => c.ClassId == schedule.ClassId);
                }
                else if (userRole == "Lecturer")
                {
                    var lecturerClasses = await _classRepository.GetByLecturerIdAsync(userId);
                    return lecturerClasses.Any(c => c.ClassId == schedule.ClassId);
                }

                return false;
            }

            private async Task<List<Schedule>> GetUserSchedulesAsync(Guid userId)
            {
                var user = await _userRepository.GetUserById(userId);
                if (user == null) return new List<Schedule>();

                var allSchedules = await _scheduleRepository.GetAllAsync();
                var userSchedules = new List<Schedule>();

                if (user.UserRoleName == "Student")
                {
                    var studentClasses = await _classRepository.GetByStudentIdAsync(userId);
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

            private async Task<Schedule?> FindScheduleByUserSelectionAsync(Guid userId, DateTime date, string slotName, string className, string userRole)
            {
                // First try to find in regular Schedule
                var todaySchedules = await GetUserTodaySchedulesAsync(userId, userRole);
                
                _logger.LogInformation("Debug - Looking for schedule with SlotName: '{SlotName}', ClassName: '{ClassName}'", slotName, className);
                _logger.LogInformation("Debug - Found {Count} regular schedules for user {UserId}", todaySchedules.Count, userId);
                
                foreach (var schedule in todaySchedules)
                {
                    var scheduleSlotName = schedule.Class?.ScheduleType?.Slot?.SlotName;
                    var scheduleClassName = schedule.Class?.ClassName;
                    
                    _logger.LogInformation("Debug - Regular Schedule {ScheduleId}: SlotName='{ScheduleSlot}', ClassName='{ScheduleClassName}'", 
                        schedule.ScheduleId, scheduleSlotName, scheduleClassName);
                    
                    if (scheduleSlotName == slotName && scheduleClassName == className)
                    {
                        _logger.LogInformation("Debug - Found matching regular schedule: {ScheduleId}", schedule.ScheduleId);
                        return schedule;
                    }
                }
                
                // If not found in regular Schedule, try to find in RegistrationSchedule and convert to Schedule
                _logger.LogInformation("Debug - No regular schedule found, trying RegistrationSchedule...");
                var todayRegistrationSchedules = await GetUserTodaySchedulesWithIncludesAsync(userId, userRole);
                
                _logger.LogInformation("Debug - Found {Count} registration schedules for user {UserId}", todayRegistrationSchedules.Count, userId);
                
                foreach (var regSchedule in todayRegistrationSchedules)
                {
                    var scheduleSlotName = regSchedule.Slot?.SlotName;
                    var scheduleClassName = regSchedule.Class?.ClassName;
                    
                    _logger.LogInformation("Debug - RegistrationSchedule {ScheduleId}: SlotName='{ScheduleSlot}', ClassName='{ScheduleClassName}'", 
                        regSchedule.RegistrationScheduleId, scheduleSlotName, scheduleClassName);
                    
                    if (scheduleSlotName == slotName && scheduleClassName == className)
                    {
                        _logger.LogInformation("Debug - Found matching registration schedule: {ScheduleId}, creating Schedule object", regSchedule.RegistrationScheduleId);
                        
                        // Create a Schedule object from RegistrationSchedule for reporting
                        var convertedSchedule = new Schedule
                        {
                            ScheduleId = regSchedule.RegistrationScheduleId, // Use RegistrationScheduleId as ScheduleId
                            ClassId = regSchedule.ClassId,
                            Class = regSchedule.Class,
                            ScheduleName = regSchedule.RegistrationScheduleName,
                            ScheduleDescription = regSchedule.RegistrationScheduleDescription
                        };
                        
                        return convertedSchedule;
                    }
                }
                
                _logger.LogWarning("Debug - No matching schedule found in either Schedule or RegistrationSchedule for SlotName: '{SlotName}', ClassName: '{ClassName}'", slotName, className);
                return null;
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
