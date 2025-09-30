using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.StudentDashboard;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Service.Implement
{
    public class StudentDashboardService : IStudentDashboardService
    {
        private readonly IClassUserRepository _classUserRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IReportRepository _reportRepository;
        private readonly ILabRepository _labRepository;
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IKitTemplateRepository _kitTemplateRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamUserRepository _teamUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public StudentDashboardService(
            IClassUserRepository classUserRepository,
            IScheduleRepository scheduleRepository,
            IGradeRepository gradeRepository,
            IReportRepository reportRepository,
            ILabRepository labRepository,
            IEquipmentTypeRepository equipmentTypeRepository,
            IKitTemplateRepository kitTemplateRepository,
            ITeamRepository teamRepository,
            ITeamUserRepository teamUserRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _classUserRepository = classUserRepository;
            _scheduleRepository = scheduleRepository;
            _gradeRepository = gradeRepository;
            _reportRepository = reportRepository;
            _labRepository = labRepository;
            _equipmentTypeRepository = equipmentTypeRepository;
            _kitTemplateRepository = kitTemplateRepository;
            _teamRepository = teamRepository;
            _teamUserRepository = teamUserRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<StudentDashboardModel>> GetStudentDashboardAsync(Guid studentId)
        {
            try
            {
                // Validation: Check if student exists
                var student = await _userRepository.GetUserById(studentId);
                if (student == null)
                {
                    return new BaseResponse<StudentDashboardModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Sinh viên không tồn tại!"
                    };
                }

                // Get student's classes
                var studentClasses = await _classUserRepository.GetByUserIdAsync(studentId);
                var classIds = studentClasses.Select(cu => cu.ClassId).ToList();

                // Get upcoming schedules
                var upcomingSchedules = await GetUpcomingSchedulesAsync(studentId, classIds);

                // Get assignments
                var assignments = await GetAssignmentsAsync(studentId);

                // Get grade summary
                var gradeSummary = await GetGradeSummaryAsync(studentId);

                // Get recent incidents
                var recentIncidents = await GetRecentIncidentsAsync(studentId, classIds);

                // Get notification summary
                var notifications = await GetNotificationSummaryAsync(studentId, assignments);

                var dashboard = new StudentDashboardModel
                {
                    StudentInfo = new StudentInfoModel
                    {
                        UserId = studentId,
                        StudentName = student.UserFullName,
                        StudentEmail = student.UserEmail,
                        TotalEnrolledClasses = studentClasses.Count(),
                        TotalTeams = await GetTotalTeamsAsync(studentId)
                    },
                    UpcomingSchedules = upcomingSchedules,
                    Assignments = assignments,
                    GradeSummary = gradeSummary,
                    RecentIncidents = recentIncidents,
                    Notifications = notifications
                };

                return new BaseResponse<StudentDashboardModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy dashboard sinh viên thành công!",
                    Data = dashboard
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<StudentDashboardModel>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi server: " + ex.Message
                };
            }
        }

        public async Task<BaseResponse<LabInstructionModel>> GetLabInstructionsAsync(int labId, Guid studentId)
        {
            try
            {
                // Validation: Check if student exists
                var student = await _userRepository.GetUserById(studentId);
                if (student == null)
                {
                    return new BaseResponse<LabInstructionModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Sinh viên không tồn tại!"
                    };
                }

                // Validation: Check if lab exists
                var lab = await _labRepository.GetLabById(labId);
                if (lab == null)
                {
                    return new BaseResponse<LabInstructionModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Lab không tồn tại!"
                    };
                }

                // Validation: Check if student has access to this lab
                var hasAccess = await CheckStudentLabAccessAsync(studentId, labId);
                if (!hasAccess)
                {
                    return new BaseResponse<LabInstructionModel>
                    {
                        Code = 403,
                        Success = false,
                        Message = "Sinh viên không có quyền truy cập lab này!"
                    };
                }

                // Get lab with related data
                var labs = await _labRepository.GetAllLabs();
                var labWithDetails = labs
                    .FirstOrDefault(l => l.LabId == labId);


                if (labWithDetails == null)
                {
                    return new BaseResponse<LabInstructionModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy thông tin chi tiết lab!"
                    };
                }

                var instructions = new LabInstructionModel
                {
                    LabId = labWithDetails.LabId,
                    LabName = labWithDetails.LabName,
                    LabTarget = labWithDetails.LabTarget,
                    LabRequest = labWithDetails.LabRequest,
                    LabStatus = labWithDetails.LabStatus,
                    SubjectName = labWithDetails.Subject.SubjectName,
                    SubjectCode = labWithDetails.Subject.SubjectCode,
                    RequiredEquipments = labWithDetails.LabEquipmentTypes.Select(let => new EquipmentInstructionModel
                    {
                        EquipmentTypeId = let.EquipmentTypeId,
                        EquipmentTypeName = let.EquipmentType.EquipmentTypeName,
                        EquipmentTypeCode = let.EquipmentType.EquipmentTypeCode,
                        EquipmentTypeDescription = let.EquipmentType.EquipmentTypeDescription,
                        EquipmentTypeQuantity = let.EquipmentType.EquipmentTypeQuantity,
                        EquipmentTypeUrlImg = let.EquipmentType.EquipmentTypeUrlImg,
                        EquipmentTypeStatus = let.EquipmentType.EquipmentTypeStatus
                    }).ToList(),
                    RequiredKits = labWithDetails.LabKitTemplates.Select(lkt => new KitInstructionModel
                    {
                        KitTemplateId = lkt.KitTemplateId,
                        KitTemplateName = lkt.KitTemplate.KitTemplateName,
                        KitTemplateQuantity = lkt.KitTemplate.KitTemplateQuantity,
                        KitTemplateDescription = lkt.KitTemplate.KitTemplateDescription,
                        KitTemplateUrlImg = lkt.KitTemplate.KitTemplateUrlImg,
                        KitTemplateStatus = lkt.KitTemplate.KitTemplateStatus
                    }).ToList(),
                    EstimatedDuration = "2-3 giờ",
                    DifficultyLevel = "Medium",
                    SafetyNotes = new List<SafetyNoteModel>
                    {
                        new SafetyNoteModel
                        {
                            NoteId = 1,
                            NoteTitle = "An toàn điện",
                            NoteContent = "Tắt nguồn điện trước khi tiếp xúc với thiết bị",
                            NoteType = "Safety",
                            Priority = 1
                        },
                        new SafetyNoteModel
                        {
                            NoteId = 2,
                            NoteTitle = "Bảo hộ cá nhân",
                            NoteContent = "Đeo kính bảo hộ và găng tay khi thực hành",
                            NoteType = "Safety",
                            Priority = 2
                        }
                    }
                };

                return new BaseResponse<LabInstructionModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy hướng dẫn lab thành công!",
                    Data = instructions
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<LabInstructionModel>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi server: " + ex.Message
                };
            }
        }

        public async Task<BaseResponse<List<EnhancedScheduleModel>>> GetEnhancedScheduleAsync(
            Guid studentId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                // Validation: Check if student exists
                var student = await _userRepository.GetUserById(studentId);
                if (student == null)
                {
                    return new BaseResponse<List<EnhancedScheduleModel>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Sinh viên không tồn tại!"
                    };
                }

                // Set default date range if not provided
                var start = startDate ?? DateTime.Today.AddDays(-7);
                var end = endDate ?? DateTime.Today.AddDays(30);

                // Validation: Check date range
                if (start > end)
                {
                    return new BaseResponse<List<EnhancedScheduleModel>>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Ngày bắt đầu không thể lớn hơn ngày kết thúc!"
                    };
                }

                // Get student's classes
                var studentClasses = await _classUserRepository.GetByUserIdAsync(studentId);
                var classIds = studentClasses.Select(cu => cu.ClassId).ToList();

                // Get schedules with enhanced information
                var allSchedules = await _scheduleRepository.GetAllAsync();
                var schedules = allSchedules
                    .Where(s => classIds.Contains(s.ClassId) && 
                               s.ScheduleDate >= start && 
                               s.ScheduleDate <= end)
                    .OrderBy(s => s.ScheduleDate)
                    .ToList();

                var enhancedSchedules = new List<EnhancedScheduleModel>();

                foreach (var schedule in schedules)
                {
                    // Extract lab info from schedule description
                    var labInfo = ExtractLabInfoFromSchedule(schedule.ScheduleDescription);
                    
                    // Get team info
                    var teamInfo = await GetTeamInfoForScheduleAsync(schedule.ClassId, studentId);
                    
                    // Get assignment status
                    var assignmentStatus = await GetAssignmentStatusForScheduleAsync(schedule.ScheduleId, studentId);
                    
                    // Get related incidents
                    var relatedIncidents = schedule.Reports.Select(r => new IncidentSummaryModel
                    {
                        ReportId = r.ReportId,
                        ReportTitle = r.ReportTitle,
                        ReportDescription = r.ReportDescription,
                        ReportCreateDate = r.ReportCreateDate,
                        ReportStatus = r.ReportStatus,
                        ReporterUserId = r.UserId,
                        ReporterName = r.User.UserFullName
                    }).ToList();

                    var enhancedSchedule = new EnhancedScheduleModel
                    {
                        ScheduleId = schedule.ScheduleId,
                        ScheduleDate = schedule.ScheduleDate,
                        ScheduleName = schedule.ScheduleName,
                        ScheduleDescription = schedule.ScheduleDescription,
                        ScheduleDetails = new ScheduleDetailModel
                        {
                            ClassId = schedule.Class.ClassId,
                            ClassName = schedule.Class.ClassName,
                            ClassDescription = schedule.Class.ClassDescription,
                            SubjectId = schedule.Class.Subject.SubjectId,
                            SubjectName = schedule.Class.Subject.SubjectName,
                            SubjectCode = schedule.Class.Subject.SubjectCode,
                            LecturerId = schedule.Class.LecturerId ?? Guid.Empty,
                            LecturerName = schedule.Class.Lecturer?.UserFullName ?? "",
                            LecturerEmail = schedule.Class.Lecturer?.UserEmail ?? "",
                            SlotId = schedule.Class.ScheduleType?.SlotId ?? 0,
                            SlotName = schedule.Class.ScheduleType?.Slot?.SlotName ?? "",
                            SlotStartTime = schedule.Class.ScheduleType?.Slot?.SlotStartTime ?? "",
                            SlotEndTime = schedule.Class.ScheduleType?.Slot?.SlotEndTime ?? "",


                        },
                        LabInfo = labInfo,
                        TeamInfo = teamInfo,
                        EquipmentSummary = await GetEquipmentSummaryForLabAsync(labInfo.LabId),
                        AssignmentStatus = assignmentStatus,
                        RelatedIncidents = relatedIncidents,
                        IsPastSchedule = schedule.ScheduleDate < DateTime.Today,
                        IsTodaySchedule = schedule.ScheduleDate.Date == DateTime.Today,
                        DaysUntilSchedule = (schedule.ScheduleDate - DateTime.Today).Days
                    };

                    enhancedSchedules.Add(enhancedSchedule);
                }

                return new BaseResponse<List<EnhancedScheduleModel>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy lịch học chi tiết thành công!",
                    Data = enhancedSchedules
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<EnhancedScheduleModel>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi server: " + ex.Message
                };
            }
        }

        #region Private Helper Methods

        private async Task<List<UpcomingScheduleModel>> GetUpcomingSchedulesAsync(Guid studentId, List<int> classIds)
        {
            var schedules = await _scheduleRepository.GetAllAsync();
            var schedule2 =schedules .Where(s => classIds.Contains(s.ClassId) && 
                           s.ScheduleDate >= DateTime.Today)
                .OrderBy(s => s.ScheduleDate)
                .Take(10)
                .ToList();

            return schedules.Select(s => new UpcomingScheduleModel
            {
                ScheduleId = s.ScheduleId,
                ScheduleDate = s.ScheduleDate,
                ScheduleName = s.ScheduleName,
                ClassId = s.Class.ClassId,
                ClassName = s.Class.ClassName,
                SlotId = s.Class.ScheduleType?.SlotId ?? 0,
                SlotName = s.Class.ScheduleType?.Slot?.SlotName ?? "",
                SlotStartTime = s.Class.ScheduleType?.Slot?.SlotStartTime ?? "",
                SlotEndTime = s.Class.ScheduleType?.Slot?.SlotEndTime ?? "",
                LabName = ExtractLabNameFromDescription(s.ScheduleDescription),
                LabTarget = ExtractLabTargetFromDescription(s.ScheduleDescription),
                LecturerName = s.Class.Lecturer?.UserFullName ?? "",
                SubjectName = s.Class.Subject.SubjectName,
                DaysUntilSchedule = (s.ScheduleDate - DateTime.Today).Days
            }).ToList();
        }

        private async Task<List<AssignmentStatusModel>> GetAssignmentsAsync(Guid studentId)
        {
            var grades = await _gradeRepository.GetAllAsync();
            var grades2 = grades
               
                .Where(g => g.UserId == studentId)
                .OrderByDescending(g => g.LabId)
                .Take(10)
                .ToList();

            return grades.Select(g => new AssignmentStatusModel
            {
                GradeId = g.GradeId,
                LabId = g.LabId,
                LabName = g.Lab.LabName,
                GradeStatus = g.GradeStatus,
                GradeScore = g.Grade1,
                GradeDescription = g.GradeDescription,
                SubmittedDate = g.GradeStatus == "Submitted" ? DateTime.Now : null,
                GradedDate = g.GradeStatus == "Graded" ? DateTime.Now : null,
                TeamId = g.TeamId,
                TeamName = g.Team.TeamName,
                SubjectName = g.Team.Class.Subject.SubjectName,
                IsOverdue = g.GradeStatus == "NotSubmitted" && DateTime.Now > GetDeadlineForLab(g.LabId),
                DaysUntilDeadline = g.GradeStatus == "NotSubmitted" ? 
                    (GetDeadlineForLab(g.LabId) - DateTime.Now).Days : 0
            }).ToList();
        }

        private async Task<GradeSummaryModel> GetGradeSummaryAsync(Guid studentId)
        {
            var grades = await _gradeRepository.GetAllAsync();
            var grades2 = grades
                
                .Where(g => g.UserId == studentId && g.Grade1 !=0)
                .ToList();

            if (!grades.Any())
            {
                return new GradeSummaryModel
                {
                    AverageScore = 0,
                    TotalAssignments = 0,
                    CompletedAssignments = 0,
                    PendingAssignments = 0,
                    OverdueAssignments = 0,
                    CompletionRate = 0,
                    HighestGradeSubject = "",
                    LowestGradeSubject = ""
                };
            }

            var allGrades = await _gradeRepository.GetAllAsync();
            var totalAssignments = allGrades.Count(g => g.UserId == studentId);

            var completedAssignments = grades.Count();
            var pendingAssignments = totalAssignments - completedAssignments;
            var averageScore = grades.Average(g => g.Grade1);
            var completionRate = totalAssignments > 0 ? (double)completedAssignments / totalAssignments * 100 : 0;

            var subjectScores = grades
                .GroupBy(g => g.Team.Class.Subject.SubjectName)
                .Select(g => new { Subject = g.Key, AverageScore = g.Average(x => x.Grade1) })
                .ToList();

            return new GradeSummaryModel
            {
                AverageScore = Math.Round(averageScore, 2),
                TotalAssignments = totalAssignments,
                CompletedAssignments = completedAssignments,
                PendingAssignments = pendingAssignments,
                OverdueAssignments = pendingAssignments, // Simplified for now
                CompletionRate = Math.Round(completionRate, 2),
                HighestGradeSubject = subjectScores.OrderByDescending(x => x.AverageScore).FirstOrDefault()?.Subject ?? "",
                LowestGradeSubject = subjectScores.OrderBy(x => x.AverageScore).FirstOrDefault()?.Subject ?? ""
            };
        }

        private async Task<List<IncidentAlertModel>> GetRecentIncidentsAsync(Guid studentId, List<int> classIds)
        {
            var incidents = await _reportRepository.GetAllAsync();
            var incidents2 = incidents
                
                .Where(r => classIds.Contains(r.Schedule.ClassId) && 
                           r.ReportCreateDate >= DateTime.Today.AddDays(-30))
                .OrderByDescending(r => r.ReportCreateDate)
                .Take(5)
                .ToList();

            return incidents.Select(r => new IncidentAlertModel
            {
                ReportId = r.ReportId,
                ReportTitle = r.ReportTitle,
                ReportDescription = r.ReportDescription,
                ReportCreateDate = r.ReportCreateDate,
                ReportStatus = r.ReportStatus,
                ScheduleId = r.ScheduleId ?? 0,
                ScheduleName = r.Schedule.ScheduleName,
                ScheduleDate = r.Schedule.ScheduleDate,
                ClassName = r.Schedule.Class.ClassName,
                DaysSinceIncident = (DateTime.Today - r.ReportCreateDate.Date).Days
            }).ToList();
        }

        private async Task<NotificationSummaryModel> GetNotificationSummaryAsync(Guid studentId, List<AssignmentStatusModel> assignments)
        {
            var upcomingDeadlines = assignments.Count(a => a.GradeStatus == "NotSubmitted" && a.DaysUntilDeadline <= 3);
            var pendingActions = assignments.Count(a => a.GradeStatus == "NotSubmitted");
            var newGrades = assignments.Count(a => a.GradeStatus == "Graded" && a.GradedDate.HasValue && 
                                                 (DateTime.Now - a.GradedDate.Value).TotalDays <= 1);

            return new NotificationSummaryModel
            {
                UnreadNotifications = 0, // Simplified for now
                UpcomingDeadlines = upcomingDeadlines,
                PendingActions = pendingActions,
                NewGrades = newGrades
            };
        }

        private async Task<int> GetTotalTeamsAsync(Guid studentId)
        {
            var allTeamUsers = await _teamUserRepository.GetAllAsync();
            return allTeamUsers.Count(tu => tu.UserId == studentId);
        }

        private async Task<bool> CheckStudentLabAccessAsync(Guid studentId, int labId)
        {
            var studentClasses = await _classUserRepository.GetByUserIdAsync(studentId);
            var classIds = studentClasses.Select(cu => cu.ClassId).ToList();

            // Check if any of the student's classes have schedules with this lab
            var allSchedules = await _scheduleRepository.GetAllAsync();
            return allSchedules.Any(s => classIds.Contains(s.ClassId) && 
                                      s.ScheduleDescription.Contains($"LabId: {labId}"));
        }

        private LabBasicInfoModel ExtractLabInfoFromSchedule(string scheduleDescription)
        {
            // Parse lab info from schedule description
            // Format: "Thực hành: {lab.LabRequest} | ScheduleTypeId: {scheduleTypeId}"
            var labInfo = new LabBasicInfoModel
            {
                LabId = 0, // Default, can be parsed if needed
                LabName = "Lab Thực Hành",
                LabTarget = "Mục tiêu lab",
                LabRequest = "Nội dung thực hành",
                LabStatus = "Active"
            };

            if (!string.IsNullOrEmpty(scheduleDescription))
            {
                // Extract lab request from description
                if (scheduleDescription.Contains("Thực hành:"))
                {
                    var parts = scheduleDescription.Split('|');
                    if (parts.Length > 0)
                    {
                        var labRequestPart = parts[0].Replace("Thực hành:", "").Trim();
                        labInfo.LabRequest = labRequestPart;
                    }
                }
            }

            return labInfo;
        }

        private string ExtractLabNameFromDescription(string scheduleDescription)
        {
            if (string.IsNullOrEmpty(scheduleDescription))
                return "Lab Thực Hành";

            if (scheduleDescription.Contains("Thực hành:"))
            {
                var parts = scheduleDescription.Split('|');
                if (parts.Length > 0)
                {
                    var labRequestPart = parts[0].Replace("Thực hành:", "").Trim();
                    return labRequestPart.Length > 50 ? labRequestPart.Substring(0, 50) + "..." : labRequestPart;
                }
            }

            return "Lab Thực Hành";
        }

        private string ExtractLabTargetFromDescription(string scheduleDescription)
        {
            // For now, return a default target
            return "Hoàn thành bài thực hành theo yêu cầu";
        }

        private DateTime GetDeadlineForLab(int labId)
        {
            // Simplified deadline calculation
            return DateTime.Now.AddDays(7);
        }

        private async Task<TeamInfoModel> GetTeamInfoForScheduleAsync(int classId, Guid studentId)
        {
            var allTeamUsers = await _teamUserRepository.GetAllAsync();
            var teamUser = allTeamUsers
                .FirstOrDefault(tu => tu.UserId == studentId && tu.Team.ClassId == classId);

            if (teamUser == null)
            {
                return new TeamInfoModel
                {
                    TeamId = 0,
                    TeamName = "Chưa phân nhóm",
                    MemberCount = 0,
                    Members = new List<TeamMemberModel>()
                };
            }

            var allTeamMembers = await _teamUserRepository.GetAllAsync();
            var teamMembers = allTeamMembers
                .Where(tu => tu.TeamId == teamUser.TeamId)
                .ToList();

            return new TeamInfoModel
            {
                TeamId = teamUser.Team.TeamId,
                TeamName = teamUser.Team.TeamName,
                TeamDescription = teamUser.Team.TeamDescription,
                MemberCount = teamMembers.Count(),
                Members = teamMembers.Select(tm => new TeamMemberModel
                {
                    UserId = tm.UserId,
                    UserName = tm.User.UserFullName,
                    UserEmail = tm.User.UserEmail,
                    
                }).ToList()
            };
        }

        private async Task<ScheduleAssignmentStatusModel> GetAssignmentStatusForScheduleAsync(int scheduleId, Guid studentId)
        {
            var grade = await _gradeRepository.GetAllAsync();
            var allgrade = grade
                
                .FirstOrDefault(g => g.UserId == studentId && 
                                        g.Team.Class.Schedules.Any(s => s.ScheduleId == scheduleId));

            if (allgrade == null)
            {
                return new ScheduleAssignmentStatusModel
                {
                    GradeId = 0,
                    GradeStatus = "NotSubmitted",
                    IsSubmitted = false,
                    IsGraded = false,
                    IsOverdue = false
                };
            }

            return new ScheduleAssignmentStatusModel
            {
                GradeId = allgrade.GradeId,
                GradeStatus = allgrade.GradeStatus,
                GradeScore = allgrade.Grade1,
                GradeDescription = allgrade.GradeDescription,
                SubmittedDate = allgrade.GradeStatus == "Submitted" ? DateTime.Now : null,
                GradedDate = allgrade.GradeStatus == "Graded" ? DateTime.Now : null,
                IsSubmitted = allgrade.GradeStatus == "Submitted" || allgrade.GradeStatus == "Graded",
                IsGraded = allgrade.GradeStatus == "Graded",
                IsOverdue = allgrade.GradeStatus == "NotSubmitted" && DateTime.Now > GetDeadlineForLab(allgrade.LabId)
            };
        }

        private async Task<EquipmentSummaryModel> GetEquipmentSummaryForLabAsync(int labId)
        {
            if (labId == 0)
            {
                return new EquipmentSummaryModel
                {
                    TotalEquipmentTypes = 0,
                    TotalKitTemplates = 0,
                    EquipmentList = new List<EquipmentBasicInfoModel>(),
                    KitList = new List<KitBasicInfoModel>()
                };
            }

            var lab1 = await _labRepository.GetAllLabs();
            var lab2 = lab1
                
                .FirstOrDefault(l => l.LabId == labId);

            if (lab2 == null)
            {
                return new EquipmentSummaryModel
                {
                    TotalEquipmentTypes = 0,
                    TotalKitTemplates = 0,
                    EquipmentList = new List<EquipmentBasicInfoModel>(),
                    KitList = new List<KitBasicInfoModel>()
                };
            }

            return new EquipmentSummaryModel
            {
                TotalEquipmentTypes = lab2.LabEquipmentTypes.Count(),
                TotalKitTemplates = lab2.LabKitTemplates.Count(),
                EquipmentList = lab2.LabEquipmentTypes.Select(let => new EquipmentBasicInfoModel
                {
                    EquipmentTypeId = let.EquipmentTypeId,
                    EquipmentTypeName = let.EquipmentType.EquipmentTypeName,
                    EquipmentTypeCode = let.EquipmentType.EquipmentTypeCode,
                    EquipmentTypeQuantity = let.EquipmentType.EquipmentTypeQuantity,
                    EquipmentTypeUrlImg = let.EquipmentType.EquipmentTypeUrlImg
                }).ToList(),
                KitList = lab2.LabKitTemplates.Select(lkt => new KitBasicInfoModel
                {
                    KitTemplateId = lkt.KitTemplateId,
                    KitTemplateName = lkt.KitTemplate.KitTemplateName,
                    KitTemplateQuantity = lkt.KitTemplate.KitTemplateQuantity,
                    KitTemplateUrlImg = lkt.KitTemplate.KitTemplateUrlImg
                }).ToList()
            };
        }

        #endregion
    }
}
