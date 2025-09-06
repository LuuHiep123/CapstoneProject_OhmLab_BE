using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(IAnalyticsService analyticsService, ILogger<AnalyticsController> logger)
        {
            _analyticsService = analyticsService;
            _logger = logger;
        }

        // Báo cáo sử dụng lab theo khoảng thời gian
        [HttpGet("lab-usage")]
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        public async Task<IActionResult> GetLabUsageReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int? subjectId = null)
        {
            try
            {
                if (startDate > endDate)
                {
                    return BadRequest(new { Code = 400, Success = false, Message = "Ngày bắt đầu không thể lớn hơn ngày kết thúc!" });
                }

                var result = await _analyticsService.GetLabUsageReportAsync(startDate, endDate, subjectId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLabUsageReport: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // Báo cáo sử dụng lab theo tháng
        [HttpGet("lab-usage/monthly/{year}/{month}")]
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        public async Task<IActionResult> GetLabUsageMonthly(int year, int month)
        {
            try
            {
                if (year < 2020 || year > 2030 || month < 1 || month > 12)
                {
                    return BadRequest(new { Code = 400, Success = false, Message = "Năm hoặc tháng không hợp lệ!" });
                }

                var result = await _analyticsService.GetLabUsageMonthlyAsync(year, month);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLabUsageMonthly: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // Chi tiết sử dụng lab
        [HttpGet("lab-usage/details")]
        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        public async Task<IActionResult> GetLabUsageDetails([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int? subjectId = null, [FromQuery] Guid? lecturerId = null)
        {
            try
            {
                if (startDate > endDate)
                {
                    return BadRequest(new { Code = 400, Success = false, Message = "Ngày bắt đầu không thể lớn hơn ngày kết thúc!" });
                }

                // Lecturer chỉ được xem báo cáo của chính mình
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
                if (currentUserRole == "Lecturer")
                {
                    var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (Guid.TryParse(currentUserId, out Guid userId))
                    {
                        lecturerId = userId;
                    }
                    else
                    {
                        return Unauthorized(new { Code = 401, Success = false, Message = "Không xác định được người dùng!" });
                    }
                }

                var result = await _analyticsService.GetLabUsageDetailAsync(startDate, endDate, subjectId, lecturerId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLabUsageDetails: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // Báo cáo sử dụng lab theo kỳ học
        [HttpGet("lab-usage/semester/{semesterId}")]
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        public async Task<IActionResult> GetLabUsageBySemester(int semesterId)
        {
            try
            {
                if (semesterId <= 0)
                {
                    return BadRequest(new { Code = 400, Success = false, Message = "Semester ID không hợp lệ!" });
                }

                var result = await _analyticsService.GetLabUsageBySemesterAsync(semesterId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLabUsageBySemester: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // Báo cáo nhanh - 7 ngày gần nhất
        [HttpGet("lab-usage/recent")]
        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        public async Task<IActionResult> GetRecentLabUsage()
        {
            try
            {
                var endDate = DateTime.Now.Date;
                var startDate = endDate.AddDays(-7);

                var result = await _analyticsService.GetLabUsageReportAsync(startDate, endDate);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRecentLabUsage: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // Báo cáo tháng hiện tại
        [HttpGet("lab-usage/current-month")]
        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        public async Task<IActionResult> GetCurrentMonthUsage()
        {
            try
            {
                var now = DateTime.Now;
                var result = await _analyticsService.GetLabUsageMonthlyAsync(now.Year, now.Month);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCurrentMonthUsage: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }
    }
}
