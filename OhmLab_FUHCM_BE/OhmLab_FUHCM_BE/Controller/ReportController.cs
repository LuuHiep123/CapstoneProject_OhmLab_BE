using BusinessLayer.RequestModel.Report;
using BusinessLayer.ResponseModel.Report;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;

        public ReportController(IReportService reportService, ILogger<ReportController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        // --- Tạo báo cáo (Student/Lecturer) ---
        [Authorize(Roles = "Student,Lecturer")]
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportRequestModel model)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == Guid.Empty)
                {
                    return Unauthorized(new { Code = 401, Success = false, Message = "Không xác định được người dùng!" });
                }

                var result = await _reportService.CreateReportAsync(model, userId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateReport: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // --- Form helpers cho tạo report (hôm nay) ---

        // Lấy danh sách slot có lịch học hôm nay
        [Authorize(Roles = "Student,Lecturer")]
        [HttpGet("today-slots")]
        public async Task<IActionResult> GetTodaySlots()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == Guid.Empty)
                {
                    return Unauthorized(new { Code = 401, Success = false, Message = "Không xác định được người dùng!" });
                }

                var result = await _reportService.GetTodaySlotsAsync(userId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTodaySlots: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // Lấy danh sách lớp theo slot hôm nay
        [Authorize(Roles = "Student,Lecturer")]
        [HttpGet("today-classes")]
        public async Task<IActionResult> GetTodayClasses([FromQuery] string slotName)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == Guid.Empty)
                {
                    return Unauthorized(new { Code = 401, Success = false, Message = "Không xác định được người dùng!" });
                }

                var result = await _reportService.GetTodayClassesAsync(userId, slotName);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTodayClasses: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // --- Xem báo cáo theo ID ---
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(int id)
        {
            try
            {
                var result = await _reportService.GetReportByIdAsync(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportById: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // --- Xem chi tiết báo cáo ---
        [Authorize]
        [HttpGet("{id}/detail")]
        public async Task<IActionResult> GetReportDetail(int id)
        {
            try
            {
                var result = await _reportService.GetReportDetailAsync(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportDetail: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // --- Lấy tất cả báo cáo (Admin/HeadOfDepartment) ---
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            try
            {
                var result = await _reportService.GetAllReportsAsync();
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllReports: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // --- Cập nhật trạng thái báo cáo (Admin/HeadOfDepartment) ---
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateReportStatus(int id, [FromBody] UpdateReportStatusRequestModel model)
        {
            try
            {
                var result = await _reportService.UpdateReportStatusAsync(id, model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateReportStatus: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // --- Thống kê báo cáo (Admin/HeadOfDepartment) ---
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet("statistics")]
        public async Task<IActionResult> GetReportStatistics()
        {
            try
            {
                var result = await _reportService.GetReportStatisticsAsync();
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportStatistics: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // --- Báo cáo theo người dùng ---
        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetReportsByUser(Guid userId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var currentUserRole = GetCurrentUserRole();

                // Chỉ cho phép xem báo cáo của chính mình hoặc Admin/HeadOfDepartment
                if (currentUserId != userId && currentUserRole != "Admin" && currentUserRole != "HeadOfDepartment")
                {
                    return Forbid();
                }

                var result = await _reportService.GetReportsByUserAsync(userId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportsByUser: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // --- Báo cáo theo lịch học ---
        [Authorize]
        [HttpGet("schedule/{registrationScheduleId}")]
        public async Task<IActionResult> GetReportsByRegistrationSchedule(int registrationScheduleId)
        {
            try
            {
                var result = await _reportService.GetReportsByRegistrationScheduleAsync(registrationScheduleId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetReportsByRegistrationSchedule: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // --- Báo cáo của tôi (Student/Lecturer) ---
        [Authorize(Roles = "Student,Lecturer")]
        [HttpGet("my-reports")]
        public async Task<IActionResult> GetMyReports()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == Guid.Empty)
                {
                    return Unauthorized(new { Code = 401, Success = false, Message = "Không xác định được người dùng!" });
                }

                var result = await _reportService.GetReportsByUserAsync(userId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetMyReports: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // --- Sự cố chờ xử lý (Admin/HeadOfDepartment) ---
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingIncidents()
        {
            try
            {
                var result = await _reportService.GetPendingIncidentsAsync();
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPendingIncidents: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        // --- Sự cố đã giải quyết (Admin/HeadOfDepartment) ---
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet("resolved")]
        public async Task<IActionResult> GetResolvedIncidents()
        {
            try
            {
                var result = await _reportService.GetResolvedIncidentsAsync();
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetResolvedIncidents: {Message}", ex.Message);
                return StatusCode(500, new { Code = 500, Success = false, Message = "Lỗi hệ thống!" });
            }
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return userId;
            }
            return Guid.Empty;
        }

        private string GetCurrentUserRole()
        {
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            return roleClaim?.Value ?? "";
        }
    }
} 