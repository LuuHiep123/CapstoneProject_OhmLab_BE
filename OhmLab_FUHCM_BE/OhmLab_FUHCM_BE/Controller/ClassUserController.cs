using BusinessLayer.Service;
using BusinessLayer.RequestModel.Class;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

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

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPost("import-excel")]
        public async Task<IActionResult> ImportUsersFromExcel([FromForm] ImportExcelRequestModel request)
        {
            try
            {
                if (request.ExcelFile == null || request.ExcelFile.Length == 0)
                {
                    return BadRequest(new { Code = 400, Success = false, Message = "Vui lòng chọn file Excel!" });
                }

                // Kiểm tra kích thước file (giới hạn 10MB)
                if (request.ExcelFile.Length > 10 * 1024 * 1024)
                {
                    return BadRequest(new { Code = 400, Success = false, Message = "File quá lớn! Tối đa 10MB." });
                }

                // Kiểm tra định dạng file
                var allowedExtensions = new[] { ".xlsx", ".xls" };
                var fileExtension = Path.GetExtension(request.ExcelFile.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { Code = 400, Success = false, Message = "Chỉ chấp nhận file Excel (.xlsx, .xls)!" });
                }

                var result = await _classUserService.ImportUsersFromExcelAsync(request.ExcelFile, request.ClassId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Code = 500, Success = false, Message = $"Lỗi server: {ex.Message}" });
            }
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