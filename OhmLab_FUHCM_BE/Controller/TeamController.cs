using BusinessLayer.RequestModel.Team;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpGet("lecturer/{lecturerId}")]
        public async Task<IActionResult> GetTeamsByLecturerId(Guid lecturerId)
        {
            try
            {
                // Lấy thông tin user hiện tại từ JWT token
                var currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(currentUserIdString) || !Guid.TryParse(currentUserIdString, out var currentUserId))
                {
                    return Unauthorized("Không xác định được thông tin người dùng!");
                }

                // Validation: Lecturer chỉ được xem teams của chính mình
                if (currentUserRole == "Lecturer" && currentUserId != lecturerId)
                {
                    return Forbid();
                }

                var result = await _teamService.GetTeamsByLecturerIdAsync(lecturerId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
    }
}
