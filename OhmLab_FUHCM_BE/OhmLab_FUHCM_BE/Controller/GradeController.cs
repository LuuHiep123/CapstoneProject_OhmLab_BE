using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using BusinessLayer.Service;
using BusinessLayer.RequestModel.Assignment;
using BusinessLayer.ResponseModel.BaseResponse;

namespace OhmLab_FUHCM_BE.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;
        private readonly ILogger<GradeController> _logger;

        public GradeController(IGradeService gradeService, ILogger<GradeController> logger)
        {
            _gradeService = gradeService;
            _logger = logger;
        }

        // Giảng viên chấm điểm cho team
        [HttpPost("labs/{labId}/teams/{teamId}/grade")]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> GradeTeamLab(int labId, int teamId, [FromBody] GradeTeamLabRequestModel model)
        {
            try
            {
                // Lấy lecturerId từ token
                var lecturerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(lecturerIdClaim, out Guid lecturerId))
                {
                    return Unauthorized(new { success = false, message = "Token không hợp lệ!" });
                }

                var result = await _gradeService.GradeTeamLabAsync(model, labId, teamId, lecturerId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GradeTeamLab: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // Giảng viên chấm điểm chi tiết cho từng member
        [HttpPost("labs/{labId}/teams/{teamId}/members/{studentId}/grade")]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> GradeTeamMember(int labId, int teamId, Guid studentId, [FromBody] GradeTeamMemberRequestModel model)
        {
            try
            {
                // Lấy lecturerId từ token
                var lecturerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(lecturerIdClaim, out Guid lecturerId))
                {
                    return Unauthorized(new { success = false, message = "Token không hợp lệ!" });
                }

                var result = await _gradeService.GradeTeamMemberAsync(model, labId, teamId, studentId, lecturerId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GradeTeamMember: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // Xem danh sách team cần chấm điểm
        [HttpGet("labs/{labId}/pending-teams")]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> GetPendingTeams(int labId)
        {
            try
            {
                // Lấy lecturerId từ token
                var lecturerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(lecturerIdClaim, out Guid lecturerId))
                {
                    return Unauthorized(new { success = false, message = "Token không hợp lệ!" });
                }

                var result = await _gradeService.GetPendingTeamsAsync(labId, lecturerId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPendingTeams: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // Team xem điểm của mình
        [HttpGet("labs/{labId}/teams/{teamId}/grade")]
        [Authorize(Roles = "Student,Lecturer")]
        public async Task<IActionResult> GetTeamGrade(int labId, int teamId)
        {
            try
            {
                // Lấy studentId từ token
                var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(studentIdClaim, out Guid studentId))
                {
                    return Unauthorized(new { success = false, message = "Token không hợp lệ!" });
                }

                var result = await _gradeService.GetTeamGradeAsync(labId, teamId, studentId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTeamGrade: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // Student xem điểm cá nhân của mình
        [HttpGet("labs/{labId}/my-individual-grade")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyIndividualGrade(int labId)
        {
            try
            {
                // Lấy studentId từ token
                var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(studentIdClaim, out Guid studentId))
                {
                    return Unauthorized(new { success = false, message = "Token không hợp lệ!" });
                }

                var result = await _gradeService.GetMyIndividualGradeAsync(labId, studentId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetMyIndividualGrade: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // Xem thống kê điểm theo team (cho Lecturer)
        [HttpGet("labs/{labId}/team-grade-statistics")]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> GetTeamGradeStatistics(int labId)
        {
            try
            {
                // Lấy lecturerId từ token
                var lecturerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(lecturerIdClaim, out Guid lecturerId))
                {
                    return Unauthorized(new { success = false, message = "Token không hợp lệ!" });
                }

                var result = await _gradeService.GetTeamGradeStatisticsAsync(labId, lecturerId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTeamGradeStatistics: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        // Xem tất cả điểm của lab (cho HeadOfDepartment)
        [HttpGet("labs/{labId}/Grade-for-id")]
        [Authorize(Roles = "HeadOfDepartment,Lecturer")]
        public async Task<IActionResult> GetAllLabGrades(int labId)
        {
            try
            {
                var result = await _gradeService.GetGradeById(labId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllLabGrades: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        [HttpGet("labs/all-grades")]
        [Authorize(Roles = "HeadOfDepartment")]
        public async Task<IActionResult> GetAllLabGrades()
        {
            try
            {
                var result = await _gradeService.GetAllGrade();
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllLabGrades: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
