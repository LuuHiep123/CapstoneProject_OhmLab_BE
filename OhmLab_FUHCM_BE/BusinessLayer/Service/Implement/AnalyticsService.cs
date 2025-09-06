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
        private readonly ILogger<AnalyticsService> _logger;

        public AnalyticsService(
            IScheduleRepository scheduleRepository,
            IClassRepository classRepository,
            IUserRepository userRepository,
            ILogger<AnalyticsService> logger)
        {
            _scheduleRepository = scheduleRepository;
            _classRepository = classRepository;
            _userRepository = userRepository;
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
                // Giả sử semester có startDate và endDate
                // Nếu không có Semester entity, có thể dùng cách khác
                var startDate = new DateTime(2024, 1, 1); // Placeholder
                var endDate = new DateTime(2024, 6, 30);   // Placeholder
                
                // TODO: Implement proper semester date lookup
                _logger.LogWarning("GetLabUsageBySemesterAsync using placeholder dates. Implement proper semester lookup.");
                
                return await GetLabUsageReportAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLabUsageBySemesterAsync: {Message}", ex.Message);
                return new BaseResponse<LabUsageReportModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

    }
}
