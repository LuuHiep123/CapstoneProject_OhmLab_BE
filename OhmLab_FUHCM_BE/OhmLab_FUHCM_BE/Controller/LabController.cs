using BusinessLayer.RequestModel.Lab;
using BusinessLayer.ResponseModel.Lab;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/labs")]
    [ApiController]
    [Authorize]
    public class LabController : ControllerBase
    {
        private readonly ILabService _labService;
        private readonly IUserService _userService;

        public LabController(ILabService labService, IUserService userService)
        {
            _labService = labService;
            _userService = userService;
        }

        // ✅ Head of Department: Tạo bài lab mẫu
        [HttpPost]
        [Authorize(Roles = "HeadOfDepartment")]
        public async Task<IActionResult> CreateLab([FromBody] CreateLabRequestModel labModel)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized("Không xác định được người dùng!");
                }

                var userId = Guid.Parse(currentUserId);
                var user = await _userService.GetUserById(userId);
                
                if (user?.Data?.UserRoleName != "HeadOfDepartment")
                {
                    return Forbid("Bạn không có quyền tạo bài lab!");
                }
                
                await _labService.AddLab(labModel, userId, user.Data.UserRoleName);
                
                return Ok(new {
                    success = true,
                    message = "Tạo bài lab mẫu thành công!",
                    data = labModel
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message,
                    data = (object)null
                });
            }
        }

        // ✅ Lecturer: Xem lab cho các lớp mình phụ trách
        [HttpGet("my-classes")]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> GetLabsForMyClasses()
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized(new { message = "Không xác định được người dùng!" });
                }

                var userId = Guid.Parse(currentUserId);
                var result = await _labService.GetLabsForMyClasses(userId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    success = false,
                    message = ex.Message,
                    data = (object)null
                });
            }
        }

        // ✅ Lecturer: Tạo lịch lab cho lớp
        [HttpPost("{labId}/schedule")]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> CreateLabSchedule(int labId, [FromBody] CreateLabScheduleRequestModel model)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized(new { message = "Không xác định được người dùng!" });
                }

                var userId = Guid.Parse(currentUserId);
                var result = await _labService.CreateLabSchedule(labId, model.ClassId, model.ScheduledDate, model.ScheduleTypeId, userId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    success = false,
                    message = ex.Message,
                    data = (object)null
                });
            }
        }


        // ✅ Admin/HeadOfDepartment: Xem tất cả lab
        [HttpGet]
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        public async Task<IActionResult> GetAllLabs()
        {
            var result = await _labService.GetAllLabs();
            return StatusCode(result.Code, result);
        }

        // ✅ Admin/HeadOfDepartment: Xem lab theo ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        public async Task<IActionResult> GetLabById(int id)
        {
            var lab = await _labService.GetLabById(id);
            if (lab != null)
            {
                return Ok(new {
                    success = true,
                    message = "Lấy chi tiết lab thành công!",
                    code = 200,
                    data = lab
                });
            }
            else
            {
                return NotFound(new {
                    success = false,
                    message = "Không tìm thấy lab!",
                    code = 404,
                    data = (object)null
                });
            }
        }

        // ✅ Admin/HeadOfDepartment: Cập nhật lab
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        public async Task<IActionResult> UpdateLab(int id, [FromBody] UpdateLabRequestModel labModel)
        {
            try
            {
                await _labService.UpdateLab(id, labModel);
                return Ok(new {
                    success = true,
                    message = "Cập nhật bài lab thành công!",
                    data = labModel
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message,
                    data = (object)null
                });
            }
        }

        // ✅ Admin/HeadOfDepartment: Xóa lab
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        public async Task<IActionResult> DeleteLab(int id)
        {
            try
            {
                await _labService.DeleteLab(id);
                return Ok(new {
                    success = true,
                    message = "Xóa bài lab thành công!",
                    data = (object)null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message,
                    data = (object)null
                });
            }
        }



        // ✅ Lecturer: Xem lab theo môn học
        [HttpGet("subject/{subjectId}")]
        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        public async Task<IActionResult> GetLabsBySubjectId(int subjectId)
        {
            var result = await _labService.GetLabsBySubjectId(subjectId);
            return StatusCode(result.Code, result);
        }

        // ✅ Lecturer: Xem lab theo lớp
        [HttpGet("class/{classId}")]
        
        public async Task<IActionResult> GetLabsByClassId(int classId)
        {
            var result = await _labService.GetLabsByClassId(classId);
            return StatusCode(result.Code, result);
        }
    }
}
