using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamUserController : ControllerBase
    {
        private readonly ITeamUserService _teamUserService;

        public TeamUserController(ITeamUserService teamUserService)
        {
            _teamUserService = teamUserService;
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPost("team/{teamId}/user/{userId}")]
        public async Task<IActionResult> AddUserToTeam(int teamId, Guid userId)
        {
            var result = await _teamUserService.AddUserToTeamAsync(teamId, userId);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamUserById(int id)
        {
            var result = await _teamUserService.GetTeamUserByIdAsync(id);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet("team/{teamId}")]
        public async Task<IActionResult> GetTeamUsersByTeamId(int teamId)
        {
            var result = await _teamUserService.GetTeamUsersByTeamIdAsync(teamId);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTeamUsersByUserId(Guid userId)
        {
            var result = await _teamUserService.GetTeamUsersByUserIdAsync(userId);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpDelete("{teamUserId}")]
        public async Task<IActionResult> RemoveUserFromTeam(int teamUserId)
        {
            var result = await _teamUserService.RemoveUserFromTeamAsync(teamUserId);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet("check/team/{teamId}/user/{userId}")]
        public async Task<IActionResult> IsUserInTeam(int teamId, Guid userId)
        {
            var result = await _teamUserService.IsUserInTeamAsync(userId, teamId);
            return StatusCode(result.Code, result);
        }
    }
} 