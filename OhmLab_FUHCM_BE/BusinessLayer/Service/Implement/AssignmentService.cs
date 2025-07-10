using AutoMapper;
using BusinessLayer.ResponseModel.BaseResponse;
using DataLayer.Entities;
using DataLayer.Repository;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Service.Implement
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ILabRepository _labRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AssignmentService> _logger;

        public AssignmentService(IScheduleRepository scheduleRepository, 
                              IReportRepository reportRepository, 
                              IGradeRepository gradeRepository, 
                              ITeamRepository teamRepository,
                              ILabRepository labRepository,
                              IMapper mapper,
                              ILogger<AssignmentService> logger)
        {
            _scheduleRepository = scheduleRepository;
            _reportRepository = reportRepository;
            _gradeRepository = gradeRepository;
            _teamRepository = teamRepository;
            _labRepository = labRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // Tạo lịch thực hành (Schedule)
        public async Task<BaseResponse<Schedule>> CreatePracticeScheduleAsync(Schedule schedule)
        {
            try
            {
                schedule.ScheduleName = $"Buổi thực hành - {schedule.ScheduleName}";
                await _scheduleRepository.CreateAsync(schedule);
                return new BaseResponse<Schedule>
                {
                    Code = 200,
                    Success = true,
                    Message = "Tạo lịch thực hành thành công!",
                    Data = schedule
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreatePracticeSchedule: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<Schedule>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<Schedule>> UpdatePracticeScheduleAsync(int scheduleId, Schedule schedule)
        {
            try
            {
                var existingSchedule = await _scheduleRepository.GetByIdAsync(scheduleId);
                if (existingSchedule == null)
                {
                    return new BaseResponse<Schedule>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lịch thực hành!",
                        Data = null
                    };
                }

                schedule.ScheduleId = scheduleId;
                await _scheduleRepository.UpdateAsync(schedule);
                
                return new BaseResponse<Schedule>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật lịch thực hành thành công!",
                    Data = schedule
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdatePracticeSchedule: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<Schedule>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeletePracticeScheduleAsync(int scheduleId)
        {
            try
            {
                await _scheduleRepository.DeleteAsync(scheduleId);
                
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa lịch thực hành thành công!",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeletePracticeSchedule: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = false
                };
            }
        }

        public async Task<DynamicResponse<Schedule>> GetPracticeSchedulesByClassAsync(int classId)
        {
            try
            {
                var schedules = await _scheduleRepository.GetByClassIdAsync(classId);
                
                return new DynamicResponse<Schedule>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách lịch thực hành theo lớp thành công!",
                    Data = new MegaData<Schedule>
                    {
                        PageData = schedules.ToList(),
                        PageInfo = new PagingMetaData
                        {
                            Page = 1,
                            Size = schedules.Count(),
                            TotalItem = schedules.Count(),
                            TotalPage = 1
                        },
                        SearchInfo = null
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPracticeSchedulesByClass: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new DynamicResponse<Schedule>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<Schedule>> GetPracticeSchedulesByLecturerAsync(Guid lecturerId)
        {
            try
            {
                var schedules = await _scheduleRepository.GetByLecturerIdAsync(lecturerId);
                
                return new DynamicResponse<Schedule>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách lịch thực hành theo giảng viên thành công!",
                    Data = new MegaData<Schedule>
                    {
                        PageData = schedules.ToList(),
                        PageInfo = new PagingMetaData
                        {
                            Page = 1,
                            Size = schedules.Count(),
                            TotalItem = schedules.Count(),
                            TotalPage = 1
                        },
                        SearchInfo = null
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPracticeSchedulesByLecturer: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new DynamicResponse<Schedule>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        // Sinh viên nộp bài thực hành (Report)
        public async Task<BaseResponse<Report>> SubmitPracticeReportAsync(Report report)
        {
            try
            {
                report.ReportCreateDate = DateTime.Now;
                report.ReportStatus = "Submitted";
                await _reportRepository.CreateAsync(report);
                
                return new BaseResponse<Report>
                {
                    Code = 200,
                    Success = true,
                    Message = "Nộp báo cáo thực hành thành công!",
                    Data = report
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SubmitPracticeReport: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<Report>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<Report>> GetReportByIdAsync(int reportId)
        {
            try
            {
                var report = await _reportRepository.GetByIdAsync(reportId);
                if (report == null)
                {
                    return new BaseResponse<Report>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy báo cáo!",
                        Data = null
                    };
                }

                return new BaseResponse<Report>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin báo cáo thành công!",
                    Data = report
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportById: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<Report>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<Report>> GetReportsByStudentAsync(Guid studentId)
        {
            try
            {
                var reports = await _reportRepository.GetReportsByStudentAsync(studentId);
                
                return new DynamicResponse<Report>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách báo cáo của sinh viên thành công!",
                    Data = new MegaData<Report>
                    {
                        PageData = reports.ToList(),
                        PageInfo = new PagingMetaData
                        {
                            Page = 1,
                            Size = reports.Count(),
                            TotalItem = reports.Count(),
                            TotalPage = 1
                        },
                        SearchInfo = null
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportsByStudent: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new DynamicResponse<Report>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<Report>> GetReportsByScheduleAsync(int scheduleId)
        {
            try
            {
                var reports = await _reportRepository.GetReportsByScheduleAsync(scheduleId);
                
                return new DynamicResponse<Report>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách báo cáo theo buổi học thành công!",
                    Data = new MegaData<Report>
                    {
                        PageData = reports.ToList(),
                        PageInfo = new PagingMetaData
                        {
                            Page = 1,
                            Size = reports.Count(),
                            TotalItem = reports.Count(),
                            TotalPage = 1
                        },
                        SearchInfo = null
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportsBySchedule: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new DynamicResponse<Report>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<Report>> GetReportsByLabAsync(int labId)
        {
            try
            {
                // Lấy các report liên quan đến lab thông qua schedule và class
                var allReports = await _reportRepository.GetAllAsync();
                var reportsByLab = allReports.Where(r => 
                    r.Schedule.Class.SubjectId == labId).ToList();
                
                return new DynamicResponse<Report>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách báo cáo theo bài thực hành thành công!",
                    Data = new MegaData<Report>
                    {
                        PageData = reportsByLab,
                        PageInfo = new PagingMetaData
                        {
                            Page = 1,
                            Size = reportsByLab.Count,
                            TotalItem = reportsByLab.Count,
                            TotalPage = 1
                        },
                        SearchInfo = null
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportsByLab: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new DynamicResponse<Report>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        // Giảng viên chấm điểm (Grade)
        public async Task<BaseResponse<Grade>> GradePracticeReportAsync(Grade grade)
        {
            try
            {
                // Kiểm tra tồn tại Lab
                var lab = await _labRepository.GetLabById(grade.LabId);
                if (lab == null)
                {
                    return new BaseResponse<Grade>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy Lab!",
                        Data = null
                    };
                }
                // Kiểm tra tồn tại Team
                var team = await _teamRepository.GetTeamById(grade.TeamId);
                if (team == null)
                {
                    return new BaseResponse<Grade>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy Team!",
                        Data = null
                    };
                }
                grade.GradeStatus = "Graded";
                await _gradeRepository.CreateAsync(grade);
                
                return new BaseResponse<Grade>
                {
                    Code = 200,
                    Success = true,
                    Message = "Chấm điểm thành công!",
                    Data = grade
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GradePracticeReport: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<Grade>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<Grade>> UpdateGradeAsync(int gradeId, Grade grade)
        {
            try
            {
                var existingGrade = await _gradeRepository.GetByIdAsync(gradeId);
                if (existingGrade == null)
                {
                    return new BaseResponse<Grade>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy điểm số!",
                        Data = null
                    };
                }

                grade.GradeId = gradeId;
                grade.GradeStatus = "Graded";
                await _gradeRepository.UpdateAsync(grade);
                
                return new BaseResponse<Grade>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật điểm số thành công!",
                    Data = grade
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateGrade: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<Grade>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<Grade>> GetGradeByIdAsync(int gradeId)
        {
            try
            {
                var grade = await _gradeRepository.GetByIdAsync(gradeId);
                if (grade == null)
                {
                    return new BaseResponse<Grade>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy điểm số!",
                        Data = null
                    };
                }

                return new BaseResponse<Grade>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin điểm số thành công!",
                    Data = grade
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetGradeById: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<Grade>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<Grade>> GetGradesByLabAsync(int labId)
        {
            try
            {
                var grades = await _gradeRepository.GetByLabIdAsync(labId);
                var gradedGrades = grades.Where(g => g.GradeStatus == "Graded").ToList();
                
                return new DynamicResponse<Grade>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách điểm số theo bài thực hành thành công!",
                    Data = new MegaData<Grade>
                    {
                        PageData = gradedGrades,
                        PageInfo = new PagingMetaData
                        {
                            Page = 1,
                            Size = gradedGrades.Count,
                            TotalItem = gradedGrades.Count,
                            TotalPage = 1
                        },
                        SearchInfo = null
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetGradesByLab: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new DynamicResponse<Grade>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<Grade>> GetGradesByStudentAsync(Guid studentId)
        {
            try
            {
                var grades = await _gradeRepository.GetByUserIdAsync(studentId);
                var gradedGrades = grades.Where(g => g.GradeStatus == "Graded").ToList();
                
                return new DynamicResponse<Grade>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách điểm số của sinh viên thành công!",
                    Data = new MegaData<Grade>
                    {
                        PageData = gradedGrades,
                        PageInfo = new PagingMetaData
                        {
                            Page = 1,
                            Size = gradedGrades.Count,
                            TotalItem = gradedGrades.Count,
                            TotalPage = 1
                        },
                        SearchInfo = null
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetGradesByStudent: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new DynamicResponse<Grade>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<Report>> GetUngradedReportsAsync()
        {
            try
            {
                var ungradedReports = await _reportRepository.GetUngradedReportsAsync();
                
                return new DynamicResponse<Report>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách báo cáo chưa chấm điểm thành công!",
                    Data = new MegaData<Report>
                    {
                        PageData = ungradedReports.ToList(),
                        PageInfo = new PagingMetaData
                        {
                            Page = 1,
                            Size = ungradedReports.Count(),
                            TotalItem = ungradedReports.Count(),
                            TotalPage = 1
                        },
                        SearchInfo = null
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUngradedReports: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new DynamicResponse<Report>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        // Phản hồi bài thực hành
        public async Task<BaseResponse<Report>> UpdateReportStatusAsync(int reportId, string status)
        {
            try
            {
                var report = await _reportRepository.GetByIdAsync(reportId);
                if (report == null)
                {
                    return new BaseResponse<Report>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy báo cáo!",
                        Data = null
                    };
                }

                report.ReportStatus = status;
                await _reportRepository.UpdateAsync(report);
                
                return new BaseResponse<Report>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật trạng thái báo cáo thành công!",
                    Data = report
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateReportStatus: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<Report>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<Grade>> AddFeedbackToGradeAsync(int gradeId, string feedback)
        {
            try
            {
                var grade = await _gradeRepository.GetByIdAsync(gradeId);
                if (grade == null)
                {
                    return new BaseResponse<Grade>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy điểm số!",
                        Data = null
                    };
                }

                grade.GradeDescription = feedback;
                await _gradeRepository.UpdateAsync(grade);
                
                return new BaseResponse<Grade>
                {
                    Code = 200,
                    Success = true,
                    Message = "Thêm phản hồi thành công!",
                    Data = grade
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddFeedbackToGrade: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<Grade>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<Grade>> GetGradesWithFeedbackAsync(int labId)
        {
            try
            {
                var grades = await _gradeRepository.GetByLabIdAsync(labId);
                var gradesWithFeedback = grades.Where(g => 
                    g.GradeStatus == "Graded" && !string.IsNullOrEmpty(g.GradeDescription)).ToList();
                
                return new DynamicResponse<Grade>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách điểm số có phản hồi thành công!",
                    Data = new MegaData<Grade>
                    {
                        PageData = gradesWithFeedback,
                        PageInfo = new PagingMetaData
                        {
                            Page = 1,
                            Size = gradesWithFeedback.Count,
                            TotalItem = gradesWithFeedback.Count,
                            TotalPage = 1
                        },
                        SearchInfo = null
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetGradesWithFeedback: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new DynamicResponse<Grade>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        // Thống kê
        public async Task<BaseResponse<object>> GetLabStatisticsAsync(int labId)
        {
            try
            {
                var grades = await _gradeRepository.GetByLabIdAsync(labId);
                var gradedGrades = grades.Where(g => g.GradeStatus == "Graded").ToList();
                
                var statistics = new
                {
                    TotalSubmissions = gradedGrades.Count,
                    GradedCount = gradedGrades.Count,
                    UngradedCount = grades.Count() - gradedGrades.Count
                };
                
                return new BaseResponse<object>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thống kê bài thực hành thành công!",
                    Data = statistics
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLabStatistics: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<object>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<object>> GetStudentGradeSummaryAsync(Guid studentId)
        {
            try
            {
                var grades = await _gradeRepository.GetByUserIdAsync(studentId);
                var gradedGrades = grades.Where(g => g.GradeStatus == "Graded").ToList();
                
                var summary = new
                {
                    TotalAssignments = gradedGrades.Count,
                    GradedCount = gradedGrades.Count,
                    TotalAssignmentsCount = grades.Count()
                };
                
                return new BaseResponse<object>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tổng kết điểm số sinh viên thành công!",
                    Data = summary
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetStudentGradeSummary: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<object>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<object>> GetClassPracticeSummaryAsync(int classId)
        {
            try
            {
                var schedules = await _scheduleRepository.GetByClassIdAsync(classId);
                var reports = await _reportRepository.GetAllAsync();
                var grades = await _gradeRepository.GetAllAsync();
                
                var classReports = reports.Where(r => 
                    schedules.Any(s => s.ScheduleId == r.ScheduleId)).ToList();
                
                var classGrades = grades.Where(g => 
                    classReports.Any(r => r.UserId == g.UserId)).ToList();
                
                var summary = new
                {
                    TotalSchedules = schedules.Count(),
                    TotalReports = classReports.Count,
                    TotalGrades = classGrades.Count(g => g.GradeStatus == "Graded"),
                    GradedCount = classGrades.Count(g => g.GradeStatus == "Graded")
                };
                
                return new BaseResponse<object>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tổng kết thực hành của lớp thành công!",
                    Data = summary
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetClassPracticeSummary: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<object>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<Schedule> GetScheduleByIdAsync(int scheduleId)
        {
            return await _scheduleRepository.GetByIdAsync(scheduleId);
        }
    }
} 