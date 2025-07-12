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

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        private readonly ILogger<AssignmentController> _logger;
        private readonly IClassRepository _classRepository;
        private readonly IWeekRepository _weekRepository;

        public AssignmentController(IAssignmentService assignmentService, ILogger<AssignmentController> logger, IClassRepository classRepository, IWeekRepository weekRepository)
        {
            _assignmentService = assignmentService;
            _logger = logger;
            _classRepository = classRepository;
            _weekRepository = weekRepository;
        }

        // --- Tạo lịch thực hành (Schedule) ---

        [HttpPost("schedules")]
        public async Task<IActionResult> CreatePracticeSchedule([FromBody] CreateScheduleRequestModel model)
        {
            try
            {
                var classEntity = await _classRepository.GetByIdAsync(model.ClassId);
                if (classEntity == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy lớp học!" });
                }
                var weekEntity = await _weekRepository.GetByIdAsync(model.WeeksId);
                if (weekEntity == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy tuần!" });
                }
                var schedule = new Schedule
                {
                    ClassId = model.ClassId,
                    WeeksId = model.WeeksId,
                    ScheduleName = model.ScheduleName,
                    ScheduleDate = model.ScheduleDate,
                    ScheduleDescription = model.ScheduleDescription
                };
                var result = await _assignmentService.CreatePracticeScheduleAsync(schedule);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreatePracticeSchedule: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return StatusCode(500, new { success = false, message = ex.Message, inner = ex.InnerException?.Message });
            }
        }

        [HttpPut("schedules/{id}")]
        public async Task<IActionResult> UpdatePracticeSchedule(int id, [FromBody] Schedule schedule)
        {
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

        // --- Sinh viên nộp bài thực hành (Report) ---

        [HttpPost("reports")]
        public async Task<IActionResult> SubmitPracticeReport([FromBody] SubmitReportRequestModel model)
        {
            try
            {
                var scheduleEntity = await _assignmentService.GetScheduleByIdAsync(model.ScheduleId);
                if (scheduleEntity == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy lịch thực hành!" });
                }
                var report = new Report
                {
                    UserId = model.UserId,
                    ScheduleId = model.ScheduleId,
                    ReportTitle = model.ReportTitle,
                    ReportDescription = model.ReportDescription,
                    ReportCreateDate = DateTime.Now,
                    ReportStatus = "Submitted"
                };
                var result = await _assignmentService.SubmitPracticeReportAsync(report);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SubmitPracticeReport: {Message} | Inner: {Inner}", ex.Message, ex.InnerException?.Message);
                return StatusCode(500, new { success = false, message = ex.Message, inner = ex.InnerException?.Message });
            }
        }

        [HttpGet("reports/{id}")]
        public async Task<IActionResult> GetReportById(int id)
        {
            var result = await _assignmentService.GetReportByIdAsync(id);
            return StatusCode(result.Code, result);
        }

        [HttpGet("students/{studentId}/reports")]
        public async Task<IActionResult> GetReportsByStudent(Guid studentId)
        {
            var result = await _assignmentService.GetReportsByStudentAsync(studentId);
            return StatusCode(result.Code, result);
        }

        [HttpGet("schedules/{scheduleId}/reports")]
        public async Task<IActionResult> GetReportsBySchedule(int scheduleId)
        {
            var result = await _assignmentService.GetReportsByScheduleAsync(scheduleId);
            return StatusCode(result.Code, result);
        }

        [HttpGet("labs/{labId}/reports")]
        public async Task<IActionResult> GetReportsByLab(int labId)
        {
            var result = await _assignmentService.GetReportsByLabAsync(labId);
            return StatusCode(result.Code, result);
        }

        // --- Giảng viên chấm điểm (Grade) ---

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
                return StatusCode(500, new { success = false, message = ex.Message, inner = ex.InnerException?.Message });
            }
        }

        [HttpPut("grades/{id}")]
        public async Task<IActionResult> UpdateGrade(int id, [FromBody] Grade grade)
        {
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

        [HttpGet("reports/ungraded")]
        public async Task<IActionResult> GetUngradedReports()
        {
            var result = await _assignmentService.GetUngradedReportsAsync();
            return StatusCode(result.Code, result);
        }

        // --- Phản hồi bài thực hành ---

        [HttpPut("reports/{reportId}/status")]
        public async Task<IActionResult> UpdateReportStatus(int reportId, [FromBody] string status)
        {
            var result = await _assignmentService.UpdateReportStatusAsync(reportId, status);
            return StatusCode(result.Code, result);
        }

        [HttpPut("grades/{gradeId}/feedback")]
        public async Task<IActionResult> AddFeedbackToGrade(int gradeId, [FromBody] string feedback)
        {
            var result = await _assignmentService.AddFeedbackToGradeAsync(gradeId, feedback);
            return StatusCode(result.Code, result);
        }

        [HttpGet("labs/{labId}/grades/feedback")]
        public async Task<IActionResult> GetGradesWithFeedback(int labId)
        {
            var result = await _assignmentService.GetGradesWithFeedbackAsync(labId);
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
    }
} 