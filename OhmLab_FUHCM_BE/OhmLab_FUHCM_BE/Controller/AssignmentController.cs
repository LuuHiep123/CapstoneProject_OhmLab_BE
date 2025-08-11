using BusinessLayer.Service;
using BusinessLayer.ResponseModel.BaseResponse;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BusinessLayer.RequestModel.Assignment;
using Microsoft.Extensions.Logging;
using DataLayer.Repository;
using System.Linq;
using DataLayer.Repository.Implement;
using BusinessLayer.RequestModel.Assignment;
using BusinessLayer.RequestModel.Class;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        private readonly ILogger<AssignmentController> _logger;
        private readonly IClassRepository _classRepository;
        //private readonly IWeekRepository _weekRepository;
        private readonly IUserRepositoty _teamEquipmentRepository;

        public AssignmentController(IAssignmentService assignmentService, ILogger<AssignmentController> logger, IClassRepository classRepository,IUserRepositoty userRepositoty)
        {
            _assignmentService = assignmentService;
            _logger = logger;
            _classRepository = classRepository;
            //_weekRepository = weekRepository;
            _teamEquipmentRepository = userRepositoty;
        }

        // --- Tạo lịch thực hành (Schedule) ---

        //[HttpPost("schedules")]
        //public async Task<IActionResult> CreatePracticeSchedule([FromBody] CreateScheduleRequestModel model)
        //{
        //    try
        //    {
        //        var classEntity = await _classRepository.GetByIdAsync(model.ClassId);
        //        if (classEntity == null)
        //        {
        //            return NotFound(new BaseResponse<object> { Code = 404, Success = false, Message = "Không tìm thấy lớp học!", Data = null });
        //        }
        //        //var weekEntity = await _weekRepository.GetByIdAsync(model.WeeksId);
        //        if (weekEntity == null)
        //        {
        //            return NotFound(new BaseResponse<object> { Code = 404, Success = false, Message = "Không tìm thấy tuần!", Data = null });
        //        }
        //        var schedule = new Schedule
        //        {
        //            ClassId = model.ClassId,
        //            WeeksId = model.WeeksId,
        //            ScheduleName = model.ScheduleName,
        //            ScheduleDate = model.ScheduleDate,
        //            ScheduleDescription = model.ScheduleDescription
        //        };
        //        var result = await _assignmentService.CreatePracticeScheduleAsync(schedule);
        //        return StatusCode(result.Code, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error in CreatePracticeSchedule: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
        //        return StatusCode(500, new BaseResponse<object> { Code = 500, Success = false, Message = ex.Message, Data = null });
        //    }
        //}

        [HttpPut("schedules/{id}")]
        public async Task<IActionResult> UpdatePracticeSchedule(int id, [FromBody] UpdateScheduleRequestModel model)
        {
            var schedule = new Schedule
            {
                ClassId = model.ClassId,
                ScheduleName = model.ScheduleName,
                ScheduleDate = model.ScheduleDate,
                ScheduleDescription = model.ScheduleDescription
            };
            var result = await _assignmentService.UpdatePracticeScheduleAsync(id, schedule);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("schedules/{id}")]
        public async Task<IActionResult> DeletePracticeSchedule(int id)
        {
            var result = await _assignmentService.DeletePracticeScheduleAsync(id);
            return StatusCode(result.Code, result);
        }

        [HttpGet("classes/{classId}/schedules")]
        public async Task<IActionResult> GetPracticeSchedulesByClass(int classId)
        {
            var result = await _assignmentService.GetPracticeSchedulesByClassAsync(classId);
            return StatusCode(result.Code, result);
        }

        [HttpGet("lecturers/{lecturerId}/schedules")]
        public async Task<IActionResult> GetPracticeSchedulesByLecturer(Guid lecturerId)
        {
            var result = await _assignmentService.GetPracticeSchedulesByLecturerAsync(lecturerId);
            return StatusCode(result.Code, result);
        }

        [HttpGet("schedules/date/{date:datetime}")]
        public async Task<IActionResult> GetSchedulesByDate(DateTime date)
        {
            var result = await _assignmentService.GetSchedulesByDateAsync(date);
            return StatusCode(result.Code, result);
        }

        [HttpGet("schedules/daterange")]
        public async Task<IActionResult> GetSchedulesByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _assignmentService.GetSchedulesByDateRangeAsync(startDate, endDate);
            return StatusCode(result.Code, result);
        }


        // --- Giảng viên chấm điểm (Grade) ---
        [Authorize(Roles = "Lecturer")]
        [HttpPost("grades")]

        public async Task<IActionResult> GradePracticeReport([FromBody] GradeReportRequestModel model)
        {
            try
            {
                var grade = new Grade
                {
                    UserId = model.UserId,
                    TeamId = model.TeamId,
                    LabId = model.LabId,
                    Grade1 = model.Grade,
                    GradeDescription = model.GradeDescription,
                    GradeStatus = "Graded"
                    // KHÔNG gán GradeId
                };
                var result = await _assignmentService.GradePracticeReportAsync(grade);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GradePracticeReport: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return StatusCode(500, new BaseResponse<object> { Code = 500, Success = false, Message = ex.Message, Data = null });
            }
        }

        [HttpPut("grades/{id}")]
        public async Task<IActionResult> UpdateGrade(int id, [FromBody] UpdateGradeRequestModel model)
        {
            var grade = new Grade
            {
                UserId = model.UserId,
                TeamId = model.TeamId,
                LabId = model.LabId,
                GradeDescription = model.GradeDescription,
                GradeStatus = model.GradeStatus
            };
            var result = await _assignmentService.UpdateGradeAsync(id, grade);
            return StatusCode(result.Code, result);
        }

        [HttpGet("grades/{id}")]
        public async Task<IActionResult> GetGradeById(int id)
        {
            var result = await _assignmentService.GetGradeByIdAsync(id);
            return StatusCode(result.Code, result);
        }

        [HttpGet("labs/{labId}/grades")]
        public async Task<IActionResult> GetGradesByLab(int labId)
        {
            var result = await _assignmentService.GetGradesByLabAsync(labId);
            return StatusCode(result.Code, result);
        }

        [HttpGet("students/{studentId}/grades")]
        public async Task<IActionResult> GetGradesByStudent(Guid studentId)
        {
            var result = await _assignmentService.GetGradesByStudentAsync(studentId);
            return StatusCode(result.Code, result);
        }


        // --- Thống kê ---

        [HttpGet("labs/{labId}/statistics")]
        public async Task<IActionResult> GetLabStatistics(int labId)
        {
            var result = await _assignmentService.GetLabStatisticsAsync(labId);
            return StatusCode(result.Code, result);
        }

        [HttpGet("students/{studentId}/grade-summary")]
        public async Task<IActionResult> GetStudentGradeSummary(Guid studentId)
        {
            var result = await _assignmentService.GetStudentGradeSummaryAsync(studentId);
            return StatusCode(result.Code, result);
        }

        [HttpGet("classes/{classId}/practice-summary")]
        public async Task<IActionResult> GetClassPracticeSummary(int classId)
        {
            var result = await _assignmentService.GetClassPracticeSummaryAsync(classId);
            return StatusCode(result.Code, result);
        }

        // --- Nộp bài thực hành (Grade) ---
        [HttpPost("grades/submit")]
        public async Task<IActionResult> SubmitAssignment([FromBody] GradeReportRequestModel model)
        {
            try
            {
                var grade = new Grade
                {
                    UserId = model.UserId,
                    TeamId = model.TeamId,
                    LabId = model.LabId,
                    Grade1 = 0, // Chưa chấm điểm
                    GradeDescription = model.GradeDescription, // Nội dung bài nộp
                    GradeStatus = "Submitted"
                };
                var result = await _assignmentService.SubmitAssignmentAsync(grade);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SubmitAssignment: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return StatusCode(500, new BaseResponse<object> { Code = 500, Success = false, Message = ex.Message, Data = null });
            }
        }

        // --- Class Endpoints ---
        [HttpGet("lecturer/{lecturerId}/classes")]
        public async Task<IActionResult> GetClassesByLecturer(Guid lecturerId)
        {
            var user = await _teamEquipmentRepository.GetUserById(lecturerId);
            if (user == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy giảng viên!", code = 404 });
            }
            if (user.UserRoleName != "Lecturer")
            {
                return StatusCode(403, new { success = false, message = "Người dùng không phải giảng viên!", code = 403 });
            }
            var classes = await _classRepository.GetByLecturerIdAsync(lecturerId);
            var result = classes.Select(c => new {
                c.ClassId,
                c.ClassName,
                c.ClassDescription,
                c.ClassStatus,
                c.SubjectId,
                c.LecturerId,
                StudentCount = c.ClassUsers?.Count ?? 0,
                TeamCount = c.Teams?.Count ?? 0,
                Schedules = c.Schedules?.Select(s => new {
                    s.ScheduleId,
                    s.ScheduleName,
                    s.ScheduleDate,
                    s.ScheduleDescription
                }),
                // Có thể bổ sung thêm thống kê báo cáo, điểm...
            }).ToList();
            return Ok(new
            {
                success = true,
                message = "Lấy danh sách lớp theo giảng viên thành công!",
                code = 200,
                data = result
            });
        }
    }
} 