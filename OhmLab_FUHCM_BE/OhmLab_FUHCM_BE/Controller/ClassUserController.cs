using BusinessLayer.Service;
using BusinessLayer.RequestModel.Class;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassUserController : ControllerBase
    {
        private readonly IClassUserService _classUserService;

        public ClassUserController(IClassUserService classUserService)
        {
            _classUserService = classUserService;
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPost("add")]
        public async Task<IActionResult> AddUserToClass([FromBody] AddUserToClassRequestModel model)
        {
            var result = await _classUserService.AddUserToClassAsync(model.UserId, model.ClassId);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassUserById(int id)
        {
            var result = await _classUserService.GetClassUserByIdAsync(id);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetClassUsersByClassId(int classId)
        {
            var result = await _classUserService.GetClassUsersByClassIdAsync(classId);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetClassUsersByUserId(Guid userId)
        {
            var result = await _classUserService.GetClassUsersByUserIdAsync(userId);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveUserFromClass([FromQuery] Guid userId, [FromQuery] int classId)
        {
            var result = await _classUserService.RemoveUserFromClassAsync(userId, classId);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet("check")]
        public async Task<IActionResult> IsUserInClass([FromQuery] Guid userId, [FromQuery] int classId)
        {
            var result = await _classUserService.IsUserInClassAsync(userId, classId);
            return StatusCode(result.Code, result);
        }
    }
} 