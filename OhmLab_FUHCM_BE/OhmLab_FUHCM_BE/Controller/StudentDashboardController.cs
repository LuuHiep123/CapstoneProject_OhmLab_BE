using System;
using System.Threading.Tasks;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.StudentDashboard;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/student/dashboard")]
    [Authorize(Roles = "Student")]
    [ApiController]
    public class StudentDashboardController : ControllerBase
    {
        private readonly IStudentDashboardService _studentDashboardService;
        private readonly ILogger<StudentDashboardController> _logger;

        public StudentDashboardController(
            IStudentDashboardService studentDashboardService,
            ILogger<StudentDashboardController> logger)
        {
            _studentDashboardService = studentDashboardService;
            _logger = logger;
        }

        /// <summary>
        /// Get student dashboard with comprehensive information
        /// </summary>
        /// <returns>Student dashboard data</returns>
        [HttpGet]
        public async Task<IActionResult> GetStudentDashboard()
        {
            try
            {
                var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return BadRequest(new BaseResponse<StudentDashboardModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Invalid user identification!"
                    });
                }

                var result = await _studentDashboardService.GetStudentDashboardAsync(userId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student dashboard");
                return StatusCode(500, new BaseResponse<StudentDashboardModel>
                {
                    Code = 500,
                    Success = false,
                    Message = "Internal server error occurred while getting student dashboard"
                });
            }
        }

        /// <summary>
        /// Get lab instructions for a specific lab
        /// </summary>
        /// <param name="labId">Lab ID</param>
        /// <returns>Lab instructions with equipment and safety information</returns>
        [HttpGet("labs/{labId}/instructions")]
        public async Task<IActionResult> GetLabInstructions(int labId)
        {
            try
            {
                // Validation: Check if labId is valid
                if (labId <= 0)
                {
                    return BadRequest(new BaseResponse<LabInstructionModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Lab ID phải lớn hơn 0!"
                    });
                }

                var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return BadRequest(new BaseResponse<LabInstructionModel>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Invalid user identification!"
                    });
                }

                var result = await _studentDashboardService.GetLabInstructionsAsync(labId, userId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting lab instructions for lab {LabId}", labId);
                return StatusCode(500, new BaseResponse<LabInstructionModel>
                {
                    Code = 500,
                    Success = false,
                    Message = "Internal server error occurred while getting lab instructions"
                });
            }
        }

        /// <summary>
        /// Get enhanced student schedule with detailed information
        /// </summary>
        /// <param name="startDate">Start date filter (optional)</param>
        /// <param name="endDate">End date filter (optional)</param>
        /// <returns>Enhanced schedule with lab, team, and assignment details</returns>
        [HttpGet("schedules/enhanced")]
        public async Task<IActionResult> GetEnhancedSchedule(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                // Validation: Check date range
                if (startDate.HasValue && endDate.HasValue && startDate.Value > endDate.Value)
                {
                    return BadRequest(new BaseResponse<string>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Ngày bắt đầu không thể lớn hơn ngày kết thúc!"
                    });
                }

                // Validation: Check if date range is too large
                if (startDate.HasValue && endDate.HasValue)
                {
                    var dateRange = (endDate.Value - startDate.Value).TotalDays;
                    if (dateRange > 365) // Limit to 1 year
                    {
                        return BadRequest(new BaseResponse<string>
                        {
                            Code = 400,
                            Success = false,
                            Message = "Khoảng thời gian không thể quá 365 ngày!"
                        });
                    }
                }

                var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return BadRequest(new BaseResponse<string>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Invalid user identification!"
                    });
                }

                var result = await _studentDashboardService.GetEnhancedScheduleAsync(userId, startDate, endDate);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting enhanced schedule for user");
                return StatusCode(500, new BaseResponse<string>
                {
                    Code = 500,
                    Success = false,
                    Message = "Internal server error occurred while getting enhanced schedule"
                });
            }
        }
    }
}
