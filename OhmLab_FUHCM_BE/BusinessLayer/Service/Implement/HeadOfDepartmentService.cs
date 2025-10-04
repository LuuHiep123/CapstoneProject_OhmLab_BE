using AutoMapper;
using BusinessLayer.RequestModel.HeadOfDepartment;
using BusinessLayer.ResponseModel.HeadOfDepartment;
using BusinessLayer.ResponseModel.BaseResponse;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class HeadOfDepartmentService : IHeadOfDepartmentService
    {
        private readonly IClassRepository _classRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ILabRepository _labRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IKitTemplateRepository _kitTemplateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<HeadOfDepartmentService> _logger;

        public HeadOfDepartmentService(
            IClassRepository classRepository,
            IEquipmentRepository equipmentRepository,
            ITeamRepository teamRepository,
            IUserRepository userRepository,
            IScheduleRepository scheduleRepository,
            ISubjectRepository subjectRepository,
            ILabRepository labRepository,
            ISemesterRepository semesterRepository,
            IEquipmentTypeRepository equipmentTypeRepository,
            IKitTemplateRepository kitTemplateRepository,
            IMapper mapper,
            ILogger<HeadOfDepartmentService> logger)
        {
            _classRepository = classRepository;
            _equipmentRepository = equipmentRepository;
                _teamRepository = teamRepository;
            _userRepository = userRepository;
            _scheduleRepository = scheduleRepository;
            _subjectRepository = subjectRepository;
            _labRepository = labRepository;
            _semesterRepository = semesterRepository;
            _equipmentTypeRepository = equipmentTypeRepository;
            _kitTemplateRepository = kitTemplateRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // Dashboard Overview - Chỉ giữ lại dashboard tổng quan
        public async Task<BaseResponse<DashboardOverviewResponseModel>> GetDashboardOverview()
        {
            try
            {
                List<Class> classes = await _classRepository.GetAllAsync();
                List<User> users = await _userRepository.GetAllUser();
                List<Subject> subjects = await _subjectRepository.GetAllSubjects();
                List<Schedule> schedules = await _scheduleRepository.GetAllAsync();

                var lecturers = users.Where(u => u.UserRoleName == "Lecturer").ToList();
                var pendingSchedules = new List<Schedule>(); // Empty list since we don't have approval status
                var approvedSchedules = new List<Schedule>(); // Empty list since we don't have approval status

                var recentClasses = classes.OrderByDescending(c => c.ClassId).Take(5).Select(c => new ClassSummaryModel
                {
                    ClassId = c.ClassId,
                    ClassName = c.ClassName,
                    SubjectName = subjects.FirstOrDefault(s => s.SubjectId == c.SubjectId)?.SubjectName ?? "",
                    LecturerName = users.FirstOrDefault(u => u.UserId == c.LecturerId)?.UserFullName ?? "",
                    StudentCount = 0,
                    ClassStatus = "Active" // Default value since we're creating summary
                }).ToList();

                var pendingApprovalSchedules = schedules.Take(5).Select(s => new ScheduleSummaryModel
                {
                    ScheduleId = s.ScheduleId,
                    ClassName = classes.FirstOrDefault(c => c.ClassId == s.ClassId)?.ClassName ?? "",
                    SubjectName = subjects.FirstOrDefault(sub => sub.SubjectId == classes.FirstOrDefault(c => c.ClassId == s.ClassId)?.SubjectId)?.SubjectName ?? "",
                    LecturerName = users.FirstOrDefault(u => u.UserId == classes.FirstOrDefault(c => c.ClassId == s.ClassId)?.LecturerId)?.UserFullName ?? "",
                    LabName = s.ScheduleName.Replace("Lab: ", ""),
                    ScheduledDate = s.ScheduleDate,
                    SlotName = "N/A", // Will be populated from ScheduleType
                    ApprovalStatus = "Pending", // Default status since Schedule doesn't have this property
                    ScheduleDescription = s.ScheduleDescription
                }).ToList();

                var dashboard = new DashboardOverviewResponseModel
                {
                    TotalClasses = classes.Count,
                    TotalLecturers = lecturers.Count,
                    TotalSubjects = subjects.Count,
                    TotalSchedules = schedules.Count,
                    PendingSchedules = pendingSchedules.Count,
                    ApprovedSchedules = approvedSchedules.Count,
                    RecentClasses = recentClasses,
                    PendingApprovalSchedules = pendingApprovalSchedules
                };

                return new BaseResponse<DashboardOverviewResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin tổng quan dashboard thành công!",
                    Data = dashboard
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin dashboard");
                return new BaseResponse<DashboardOverviewResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }
        // Lab Monitoring - Chỉ giữ lại các API thống kê
        public async Task<BaseResponse<LabMonitoringResponseModel>> GetLabMonitoringOverview(MonitorLabRequestModel model)
        {
            try
            {
                List<Schedule> schedules = await _scheduleRepository.GetAllAsync();
                List<Class> classes = await _classRepository.GetAllAsync();
                List<Subject> subjects = await _subjectRepository.GetAllSubjects();
                List<User> users = await _userRepository.GetAllUser();
                List<Lab> labs = await _labRepository.GetAllLabs();

                // Filter by date range if provided
                if (model.FromDate.HasValue)
                {
                    schedules = schedules.Where(s => s.ScheduleDate >= model.FromDate.Value).ToList();
                }
                if (model.ToDate.HasValue)
                {
                    schedules = schedules.Where(s => s.ScheduleDate <= model.ToDate.Value).ToList();
                }

                // Filter by subject if provided
                if (model.SubjectId.HasValue)
                {
                    var subjectClasses = classes.Where(c => c.SubjectId == model.SubjectId.Value).Select(c => c.ClassId).ToList();
                    schedules = schedules.Where(s => subjectClasses.Contains(s.ClassId)).ToList();
                }

                // Filter by class if provided
                if (model.ClassId.HasValue)
                {
                    schedules = schedules.Where(s => s.ClassId == model.ClassId.Value).ToList();
                }

                // Filter by lecturer if provided
                if (model.LecturerId.HasValue)
                {
                    var lecturerClasses = classes.Where(c => c.LecturerId == model.LecturerId.Value).Select(c => c.ClassId).ToList();
                    schedules = schedules.Where(s => lecturerClasses.Contains(s.ClassId)).ToList();
                }

                var totalSessions = schedules.Count;
                var completedSessions = 0; // Default value since Schedule doesn't have ApprovalStatus
                var pendingSessions = 0; // Default value since Schedule doesn't have ApprovalStatus
                var cancelledSessions = 0; // Default value since Schedule doesn't have ApprovalStatus

                var completionRate = 0.0; // Default value since we don't have approval status

                // Subject statistics
                var subjectStats = subjects.Select(sub => new SubjectStatisticsModel
                {
                    SubjectId = sub.SubjectId,
                    SubjectName = sub.SubjectName,
                    TotalSessions = schedules.Count(s => classes.FirstOrDefault(c => c.ClassId == s.ClassId)?.SubjectId == sub.SubjectId),
                    CompletedSessions = schedules.Count(s => classes.FirstOrDefault(c => c.ClassId == s.ClassId)?.SubjectId == sub.SubjectId), // Count all sessions as completed for now
                    CompletionRate = 0, // Will be calculated
                    TotalStudents = 0, // Will be calculated from ClassUser
                    InvolvedLecturers = classes.Where(c => c.SubjectId == sub.SubjectId)
                        .Select(c => users.FirstOrDefault(u => u.UserId == c.LecturerId)?.UserFullName ?? "")
                        .Where(name => !string.IsNullOrEmpty(name))
                        .Distinct()
                        .ToList()
                }).Where(s => s.TotalSessions > 0).ToList();

                // Calculate completion rates
                foreach (var stat in subjectStats)
                {
                    stat.CompletionRate = stat.TotalSessions > 0 ? (double)stat.CompletedSessions / stat.TotalSessions * 100 : 0;
                }

                // Lecturer performance
                var lecturerPerformance = users.Where(u => u.UserRoleName == "Lecturer").Select(lecturer => new LecturerPerformanceModel
                {
                    LecturerId = lecturer.UserId,
                    LecturerName = lecturer.UserFullName,
                    TotalAssignedClasses = classes.Count(c => c.LecturerId == lecturer.UserId),
                    TotalLabSessions = schedules.Count(s => classes.FirstOrDefault(c => c.ClassId == s.ClassId)?.LecturerId == lecturer.UserId),
                    CompletedSessions = schedules.Count(s => classes.FirstOrDefault(c => c.ClassId == s.ClassId)?.LecturerId == lecturer.UserId), // Count all sessions as completed for now
                    CompletionRate = 0, // Will be calculated
                    SubjectsTaught = classes.Where(c => c.LecturerId == lecturer.UserId)
                        .Select(c => subjects.FirstOrDefault(s => s.SubjectId == c.SubjectId)?.SubjectName ?? "")
                        .Where(name => !string.IsNullOrEmpty(name))
                        .Distinct()
                        .ToList()
                }).Where(l => l.TotalLabSessions > 0).ToList();

                // Calculate completion rates
                foreach (var lecturer in lecturerPerformance)
                {
                    lecturer.CompletionRate = lecturer.TotalLabSessions > 0 ? (double)lecturer.CompletedSessions / lecturer.TotalLabSessions * 100 : 0;
                }

                // Equipment usage statistics
                var equipmentUsage = await _equipmentRepository.GetAllEquipment();
                var equipmentStats = equipmentUsage.Select(eq => new EquipmentUsageModel
                {
                    EquipmentTypeId = eq.EquipmentTypeId,
                    EquipmentTypeName = eq.EquipmentName,
                    TotalUsage = 0, // Will be calculated from lab requirements
                    
                    UtilizationRate = 0, // Will be calculated
                    UsedInSubjects = new List<string>() // Will be populated
                }).ToList();

                // Calculate equipment usage from lab requirements
                foreach (var eqStat in equipmentStats)
                {
                    var usedInLabs = labs.Where(l => l.LabEquipmentTypes != null && l.LabEquipmentTypes.Any(re => re.EquipmentTypeId == eqStat.EquipmentTypeId)).ToList();
                    
                    // Count the number of labs using this equipment type
                    eqStat.TotalUsage = usedInLabs.Count;
                    
                    // Calculate utilization rate (assuming each lab uses one unit of the equipment)
                    eqStat.UtilizationRate = eqStat.AvailableQuantity > 0 ? 
                        (double)eqStat.TotalUsage / eqStat.AvailableQuantity * 100 : 0;
                        
                    // Get unique subject names where this equipment is used
                    eqStat.UsedInSubjects = usedInLabs
                        .Select(l => subjects.FirstOrDefault(s => s.SubjectId == l.SubjectId)?.SubjectName ?? "")
                        .Where(name => !string.IsNullOrEmpty(name))
                        .Distinct()
                        .ToList();
                }

                var monitoring = new LabMonitoringResponseModel
                {
                    TotalLabSessions = totalSessions,
                    CompletedSessions = completedSessions,
                    PendingSessions = pendingSessions,
                    CancelledSessions = cancelledSessions,
                    CompletionRate = completionRate,
                    SubjectStatistics = subjectStats,
                    LecturerPerformance = lecturerPerformance,
                    EquipmentUsage = equipmentStats
                };

                return new BaseResponse<LabMonitoringResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin giám sát lab thành công!",
                    Data = monitoring
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin giám sát lab");
                return new BaseResponse<LabMonitoringResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<SubjectStatisticsModel>>> GetSubjectStatistics()
        {
            try
            {
                List<Subject> subjects = await _subjectRepository.GetAllSubjects();
                List<Class> classes = await _classRepository.GetAllAsync();
                List<Schedule> schedules = await _scheduleRepository.GetAllAsync();

                var subjectStats = subjects.Select(sub => new SubjectStatisticsModel
                {
                    SubjectId = sub.SubjectId,
                    SubjectName = sub.SubjectName,
                    TotalSessions = schedules.Count(s => classes.FirstOrDefault(c => c.ClassId == s.ClassId)?.SubjectId == sub.SubjectId),
                    CompletedSessions = schedules.Count(s => classes.FirstOrDefault(c => c.ClassId == s.ClassId)?.SubjectId == sub.SubjectId), // Count all sessions as completed for now
                    CompletionRate = 0, // Will be calculated
                    TotalStudents = 0, // Will be calculated from ClassUser
                    InvolvedLecturers = classes.Where(c => c.SubjectId == sub.SubjectId)
                        .Select(c => c.LecturerId.ToString())
                        .Distinct()
                        .ToList()
                }).Where(s => s.TotalSessions > 0).ToList();

                // Calculate completion rates
                foreach (var stat in subjectStats)
                {
                    stat.CompletionRate = stat.TotalSessions > 0 ? (double)stat.CompletedSessions / stat.TotalSessions * 100 : 0;
                }

                return new BaseResponse<List<SubjectStatisticsModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thống kê theo môn học thành công!",
                    Data = subjectStats
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thống kê theo môn học");
                return new BaseResponse<List<SubjectStatisticsModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<LecturerPerformanceModel>>> GetLecturerPerformance()
        {
            try
            {
                List<User> users = await _userRepository.GetAllUser();
                List<Class> classes = await _classRepository.GetAllAsync();
                List<Schedule> schedules = await _scheduleRepository.GetAllAsync();
                List<Subject> subjects = await _subjectRepository.GetAllSubjects();

                var lecturerPerformance = users.Where(u => u.UserRoleName == "Lecturer").Select(lecturer => new LecturerPerformanceModel
                {
                    LecturerId = lecturer.UserId,
                    LecturerName = lecturer.UserFullName,
                    TotalAssignedClasses = classes.Count(c => c.LecturerId == lecturer.UserId),
                    TotalLabSessions = schedules.Count(s => classes.FirstOrDefault(c => c.ClassId == s.ClassId)?.LecturerId == lecturer.UserId),
                    CompletedSessions = schedules.Count(s => classes.FirstOrDefault(c => c.ClassId == s.ClassId)?.LecturerId == lecturer.UserId), // Count all sessions as completed for now
                    CompletionRate = 0, // Will be calculated
                    SubjectsTaught = classes.Where(c => c.LecturerId == lecturer.UserId)
                        .Select(c => subjects.FirstOrDefault(s => s.SubjectId == c.SubjectId)?.SubjectName ?? "")
                        .Where(name => !string.IsNullOrEmpty(name))
                        .Distinct()
                        .ToList()
                }).Where(l => l.TotalLabSessions > 0).ToList();

                // Calculate completion rates
                foreach (var lecturer in lecturerPerformance)
                {
                    lecturer.CompletionRate = lecturer.TotalLabSessions > 0 ? (double)lecturer.CompletedSessions / lecturer.TotalLabSessions * 100 : 0;
                }

                return new BaseResponse<List<LecturerPerformanceModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin hiệu suất giảng viên thành công!",
                    Data = lecturerPerformance
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin hiệu suất giảng viên");
                return new BaseResponse<List<LecturerPerformanceModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<EquipmentUsageModel>>> GetEquipmentUsage()
        {
            try
            {
                // Get all equipment with their type information
                var equipment = await _equipmentRepository.GetAllEquipment();
                var equipmentTypes = await _equipmentTypeRepository.GetAllEquipmentType();
                
                // Get all teams with their equipment
                var teams = await _teamRepository.GetAllAsync();
                var teamEquipments = teams
                    .Where(t => t.TeamEquipments != null)
                    .SelectMany(t => t.TeamEquipments)
                    .ToList();

                // Create equipment usage statistics
                var equipmentUsage = equipment.Select(eq => 
                {
                    var equipmentType = equipmentTypes.FirstOrDefault(et => et.EquipmentTypeId == eq.EquipmentTypeId);
                    
                    // Get all assignments for this equipment
                    var assignments = teamEquipments
                        .Where(te => te.EquipmentId == eq.EquipmentId)
                        .ToList();

                    // Get unique subjects where this equipment is used
                    var usedInSubjects = assignments
                        .Select(te => te.Team?.Class?.Subject?.SubjectName)
                        .Where(name => !string.IsNullOrEmpty(name))
                        .Distinct()
                        .ToList();

                    return new EquipmentUsageModel
                    {
                        EquipmentTypeId = eq.EquipmentId,
                        EquipmentTypeName = eq.EquipmentName,
                        TotalUsage = assignments.Count,
                        UtilizationRate = assignments.Count > 0 ? 100.0 : 0.0,
                        UsedInSubjects = usedInSubjects ?? new List<string>()
                    };
                }).ToList();

                return new BaseResponse<List<EquipmentUsageModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin sử dụng thiết bị thành công!",
                    Data = equipmentUsage
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin sử dụng thiết bị");
                return new BaseResponse<List<EquipmentUsageModel>>
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
