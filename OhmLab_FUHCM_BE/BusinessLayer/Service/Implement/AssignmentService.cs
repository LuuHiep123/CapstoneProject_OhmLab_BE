using AutoMapper;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Assignment;
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
       
        private readonly ILabRepository _labRepository;
        private readonly IClassRepository _classRepository;
        //private readonly IWeekRepository _weekRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AssignmentService> _logger;

        public AssignmentService(IScheduleRepository scheduleRepository, 
                              IReportRepository reportRepository, 
                              IGradeRepository gradeRepository, 
                              ILabRepository labRepository,
                              IClassRepository classRepository,
                              //IWeekRepository weekRepository,
                              IMapper mapper,
                              ILogger<AssignmentService> logger)
        {
            _scheduleRepository = scheduleRepository;
            _reportRepository = reportRepository;
            _gradeRepository = gradeRepository;
          
            _labRepository = labRepository;
            _classRepository = classRepository;
            //_weekRepository = weekRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // Tạo lịch thực hành (Schedule)
        //public async Task<BaseResponse<ScheduleResponseModel>> CreatePracticeScheduleAsync(Schedule schedule)
        //{
        //    try
        //    {
        //        // Kiểm tra lớp
        //        var classEntity = await _classRepository.GetByIdAsync(schedule.ClassId);
        //        if (classEntity == null)
        //        {
        //            return new BaseResponse<ScheduleResponseModel>
        //            {
        //                Code = 404,
        //                Success = false,
        //                Message = "Không tìm thấy lớp học!",
        //                Data = null
        //            };
        //        }
        //        // Kiểm tra tuần
        //        //var weekEntity = await _weekRepository.GetByIdAsync(schedule.WeeksId);
        //        if (weekEntity == null)
        //        {
        //            return new BaseResponse<ScheduleResponseModel>
        //            {
        //                Code = 404,
        //                Success = false,
        //                Message = "Không tìm thấy tuần!",
        //                Data = null
        //            };
        //        }
        //        // Kiểm tra trùng lịch
        //        var schedules = await _scheduleRepository.GetByClassIdAsync(schedule.ClassId);
        //        //bool isDuplicate = schedules.Any(s => s.WeeksId == schedule.WeeksId && s.ScheduleDate.Date == schedule.ScheduleDate.Date);
        //        if (isDuplicate)
        //        {
        //            return new BaseResponse<ScheduleResponseModel>
        //            {
        //                Code = 409,
        //                Success = false,
        //                Message = "Lịch thực hành đã tồn tại!",
        //                Data = null
        //            };
        //        }
        //        // Kiểm tra ngày nằm trong tuần
        //        if (schedule.ScheduleDate < weekEntity.WeeksStartDate || schedule.ScheduleDate > weekEntity.WeeksEndDate)
        //        {
        //            return new BaseResponse<ScheduleResponseModel>
        //            {
        //                Code = 400,
        //                Success = false,
        //                Message = "Ngày thực hành phải nằm trong tuần học!",
        //                Data = null
        //            };
        //        }
        //        schedule.ScheduleName = $"Buổi thực hành - {schedule.ScheduleName}";
        //        await _scheduleRepository.CreateAsync(schedule);
        //        var dto = _mapper.Map<ScheduleResponseModel>(schedule);
        //        return new BaseResponse<ScheduleResponseModel>
        //        {
        //            Code = 200,
        //            Success = true,
        //            Message = "Tạo lịch thực hành thành công!",
        //            Data = dto
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error in CreatePracticeSchedule: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
        //        return new BaseResponse<ScheduleResponseModel>
        //        {
        //            Code = 500,
        //            Success = false,
        //            Message = ex.Message,
        //            Data = null
        //        };
        //    }
        //}

        public async Task<BaseResponse<ScheduleResponseModel>> UpdatePracticeScheduleAsync(int scheduleId, Schedule schedule)
        {
            try
            {
                var existingSchedule = await _scheduleRepository.GetByIdAsync(scheduleId);
                if (existingSchedule == null)
                {
                    return new BaseResponse<ScheduleResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lịch thực hành!",
                        Data = null
                    };
                }

                schedule.ScheduleId = scheduleId;
                await _scheduleRepository.UpdateAsync(schedule);
                
                var dto = _mapper.Map<ScheduleResponseModel>(schedule);
                return new BaseResponse<ScheduleResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật lịch thực hành thành công!",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdatePracticeSchedule: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<ScheduleResponseModel>
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

        public async Task<DynamicResponse<ScheduleResponseModel>> GetPracticeSchedulesByClassAsync(int classId)
        {
            try
            {
                var schedules = await _scheduleRepository.GetByClassIdAsync(classId);
                
                return new DynamicResponse<ScheduleResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách lịch thực hành theo lớp thành công!",
                    Data = new MegaData<ScheduleResponseModel>
                    {
                        PageData = schedules.Select(s => _mapper.Map<ScheduleResponseModel>(s)).ToList(),
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
                return new DynamicResponse<ScheduleResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<ScheduleResponseModel>> GetPracticeSchedulesByLecturerAsync(Guid lecturerId)
        {
            try
            {
                var schedules = await _scheduleRepository.GetByLecturerIdAsync(lecturerId);
                
                return new DynamicResponse<ScheduleResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách lịch thực hành theo giảng viên thành công!",
                    Data = new MegaData<ScheduleResponseModel>
                    {
                        PageData = schedules.Select(s => _mapper.Map<ScheduleResponseModel>(s)).ToList(),
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
                return new DynamicResponse<ScheduleResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        // Sinh viên nộp bài thực hành (Report)
        public async Task<BaseResponse<ReportResponseModel>> SubmitPracticeReportAsync(Report report)
        {
            try
            {
                // Kiểm tra lịch thực hành tồn tại
                var scheduleEntity = await _scheduleRepository.GetByIdAsync(report.ScheduleId);
                if (scheduleEntity == null)
                {
                    return new BaseResponse<ReportResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lịch thực hành!",
                        Data = null
                    };
                }
                // Kiểm tra đã nộp chưa
                var reports = await _reportRepository.GetByUserIdAsync(report.UserId);
                bool hasSubmitted = reports.Any(r => r.ScheduleId == report.ScheduleId);
                if (hasSubmitted)
                {
                    return new BaseResponse<ReportResponseModel>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Bạn đã nộp báo cáo cho buổi này!",
                        Data = null
                    };
                }
                // Kiểm tra lịch đã kết thúc chưa
                if (scheduleEntity.ScheduleDate < DateTime.Now)
                {
                    return new BaseResponse<ReportResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Không thể nộp báo cáo cho lịch đã kết thúc!",
                        Data = null
                    };
                }
                report.ReportCreateDate = DateTime.Now;
                report.ReportStatus = "Submitted";
                await _reportRepository.CreateAsync(report);
                var dto = _mapper.Map<ReportResponseModel>(report);
                return new BaseResponse<ReportResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Nộp báo cáo thực hành thành công!",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SubmitPracticeReport: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<ReportResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

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

                var dto = _mapper.Map<ReportResponseModel>(report);
                return new BaseResponse<ReportResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin báo cáo thành công!",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportById: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<ReportResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<ReportResponseModel>> GetReportsByStudentAsync(Guid studentId)
        {
            try
            {
                var reports = await _reportRepository.GetReportsByStudentAsync(studentId);
                
                return new DynamicResponse<ReportResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách báo cáo của sinh viên thành công!",
                    Data = new MegaData<ReportResponseModel>
                    {
                        PageData = reports.Select(r => _mapper.Map<ReportResponseModel>(r)).ToList(),
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
                return new DynamicResponse<ReportResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<ReportResponseModel>> GetReportsByScheduleAsync(int scheduleId)
        {
            try
            {
                var reports = await _reportRepository.GetReportsByScheduleAsync(scheduleId);
                
                return new DynamicResponse<ReportResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách báo cáo theo buổi học thành công!",
                    Data = new MegaData<ReportResponseModel>
                    {
                        PageData = reports.Select(r => _mapper.Map<ReportResponseModel>(r)).ToList(),
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
                return new DynamicResponse<ReportResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<ReportResponseModel>> GetReportsByLabAsync(int labId)
        {
            try
            {
                // Lấy các report liên quan đến lab thông qua schedule và class
                var allReports = await _reportRepository.GetAllAsync();
                var reportsByLab = allReports.Where(r => 
                    r.Schedule.Class.SubjectId == labId).ToList();
                
                return new DynamicResponse<ReportResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách báo cáo theo bài thực hành thành công!",
                    Data = new MegaData<ReportResponseModel>
                    {
                        PageData = reportsByLab.Select(r => _mapper.Map<ReportResponseModel>(r)).ToList(),
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
                return new DynamicResponse<ReportResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        // Giảng viên chấm điểm (Grade)
        public async Task<BaseResponse<GradeResponseModel>> GradePracticeReportAsync(Grade grade)
        {
            try
            {
                // Kiểm tra báo cáo đã nộp chưa
                var reports = await _reportRepository.GetByUserIdAsync(grade.UserId);
                bool hasReport = reports.Any(r => r.ScheduleId == grade.LabId);
                if (!hasReport)
                {
                    return new BaseResponse<GradeResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Chưa có báo cáo để chấm điểm!",
                        Data = null
                    };
                }
                // Kiểm tra đã chấm chưa
                var grades = await _gradeRepository.GetByUserIdAsync(grade.UserId);
                bool hasGraded = grades.Any(g => g.LabId == grade.LabId);
                if (hasGraded)
                {
                    return new BaseResponse<GradeResponseModel>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Báo cáo này đã được chấm điểm!",
                        Data = null
                    };
                }
                grade.GradeStatus = "Graded";
                // Đảm bảo đã có điểm số
                if (grade.Grade1 < 0)
                {
                    return new BaseResponse<GradeResponseModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Điểm không hợp lệ!",
                        Data = null
                    };
                }
                await _gradeRepository.CreateAsync(grade);
                var dto = _mapper.Map<GradeResponseModel>(grade);
                return new BaseResponse<GradeResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Chấm điểm thành công!",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GradePracticeReport: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<GradeResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<GradeResponseModel>> UpdateGradeAsync(int gradeId, Grade grade)
        {
            try
            {
                var existingGrade = await _gradeRepository.GetByIdAsync(gradeId);
                if (existingGrade == null)
                {
                    return new BaseResponse<GradeResponseModel>
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
                
                var dto = _mapper.Map<GradeResponseModel>(grade);
                return new BaseResponse<GradeResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật điểm số thành công!",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateGrade: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<GradeResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<GradeResponseModel>> GetGradeByIdAsync(int gradeId)
        {
            try
            {
                var grade = await _gradeRepository.GetByIdAsync(gradeId);
                if (grade == null)
                {
                    return new BaseResponse<GradeResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy điểm số!",
                        Data = null
                    };
                }

                var dto = _mapper.Map<GradeResponseModel>(grade);
                return new BaseResponse<GradeResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin điểm số thành công!",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetGradeById: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<GradeResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<GradeResponseModel>> GetGradesByLabAsync(int labId)
        {
            try
            {
                var grades = await _gradeRepository.GetByLabIdAsync(labId);
                var gradedGrades = grades.Where(g => g.GradeStatus == "Graded").ToList();
                
                return new DynamicResponse<GradeResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách điểm số theo bài thực hành thành công!",
                    Data = new MegaData<GradeResponseModel>
                    {
                        PageData = gradedGrades.Select(g => _mapper.Map<GradeResponseModel>(g)).ToList(),
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
                return new DynamicResponse<GradeResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<GradeResponseModel>> GetGradesByStudentAsync(Guid studentId)
        {
            try
            {
                var grades = await _gradeRepository.GetByUserIdAsync(studentId);
                var gradedGrades = grades.Where(g => g.GradeStatus == "Graded").ToList();
                
                return new DynamicResponse<GradeResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách điểm số của sinh viên thành công!",
                    Data = new MegaData<GradeResponseModel>
                    {
                        PageData = gradedGrades.Select(g => _mapper.Map<GradeResponseModel>(g)).ToList(),
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
                return new DynamicResponse<GradeResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<ReportResponseModel>> GetUngradedReportsAsync()
        {
            try
            {
                var ungradedReports = await _reportRepository.GetUngradedReportsAsync();
                
                return new DynamicResponse<ReportResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách báo cáo chưa chấm điểm thành công!",
                    Data = new MegaData<ReportResponseModel>
                    {
                        PageData = ungradedReports.Select(r => _mapper.Map<ReportResponseModel>(r)).ToList(),
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
                return new DynamicResponse<ReportResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        // Phản hồi bài thực hành
        public async Task<BaseResponse<ReportResponseModel>> UpdateReportStatusAsync(int reportId, string status)
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

                report.ReportStatus = status;
                await _reportRepository.UpdateAsync(report);
                
                var dto = _mapper.Map<ReportResponseModel>(report);
                return new BaseResponse<ReportResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật trạng thái báo cáo thành công!",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateReportStatus: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<ReportResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<GradeResponseModel>> AddFeedbackToGradeAsync(int gradeId, string feedback)
        {
            try
            {
                var grade = await _gradeRepository.GetByIdAsync(gradeId);
                if (grade == null)
                {
                    return new BaseResponse<GradeResponseModel>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy điểm số!",
                        Data = null
                    };
                }

                grade.GradeDescription = feedback;
                await _gradeRepository.UpdateAsync(grade);
                
                var dto = _mapper.Map<GradeResponseModel>(grade);
                return new BaseResponse<GradeResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Thêm phản hồi thành công!",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddFeedbackToGrade: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<GradeResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<GradeResponseModel>> GetGradesWithFeedbackAsync(int labId)
        {
            try
            {
                var grades = await _gradeRepository.GetByLabIdAsync(labId);
                var gradesWithFeedback = grades.Where(g => 
                    g.GradeStatus == "Graded" && !string.IsNullOrEmpty(g.GradeDescription)).ToList();
                
                return new DynamicResponse<GradeResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách điểm số có phản hồi thành công!",
                    Data = new MegaData<GradeResponseModel>
                    {
                        PageData = gradesWithFeedback.Select(g => _mapper.Map<GradeResponseModel>(g)).ToList(),
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
                return new DynamicResponse<GradeResponseModel>
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

        public async Task<BaseResponse<GradeResponseModel>> SubmitAssignmentAsync(Grade grade)
        {
            try
            {
                // Kiểm tra đã nộp chưa (theo UserId, TeamId, LabId, status Submitted)
                var grades = await _gradeRepository.GetByUserIdAsync(grade.UserId);
                bool hasSubmitted = grades.Any(g => g.LabId == grade.LabId && g.GradeStatus == "Submitted");
                if (hasSubmitted)
                {
                    return new BaseResponse<GradeResponseModel>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Bạn đã nộp bài cho lab này!",
                        Data = null
                    };
                }
                grade.GradeStatus = "Submitted";
                grade.Grade1 = 0;
                var created = await _gradeRepository.CreateAsync(grade);
                var dto = _mapper.Map<GradeResponseModel>(created);
                return new BaseResponse<GradeResponseModel>
                {
                    Code = 200,
                    Success = true,
                    Message = "Nộp bài thực hành thành công!",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SubmitAssignmentAsync: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return new BaseResponse<GradeResponseModel>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
    }
} 