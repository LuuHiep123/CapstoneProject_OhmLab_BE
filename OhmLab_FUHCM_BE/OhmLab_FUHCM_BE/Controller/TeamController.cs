using BusinessLayer.RequestModel.Team;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamRequestModel model)
        {
            var result = await _teamService.CreateTeamAsync(model);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById(int id)
        {
            var result = await _teamService.GetTeamByIdAsync(id);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpGet]
        public async Task<IActionResult> GetAllTeams()
        {
            var result = await _teamService.GetAllTeamsAsync();
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetTeamsByClassId(int classId)
        {
            var result = await _teamService.GetTeamsByClassIdAsync(classId);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] UpdateTeamRequestModel model)
        {
            var result = await _teamService.UpdateTeamAsync(id, model);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var result = await _teamService.DeleteTeamAsync(id);
            return StatusCode(result.Code, result);
        }
    }
} 