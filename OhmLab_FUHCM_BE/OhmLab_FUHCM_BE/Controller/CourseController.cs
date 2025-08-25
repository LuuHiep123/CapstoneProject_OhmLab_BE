using BusinessLayer.RequestModel.Lab;
using BusinessLayer.RequestModel.Subject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Lab;
using BusinessLayer.ResponseModel.Subject;
using System.Collections.Generic;
using DataLayer.Repository;
using DataLayer.Entities;
using System.Security.Claims;
using BusinessLayer.Service;
namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly ILabService _labService;
        private readonly IClassRepository _classRepository;
        private readonly IUserService _userService;

        public CourseController(ISubjectService subjectService, ILabService labService, IClassRepository classRepository, IUserService userService)
        {
            _subjectService = subjectService;
            _labService = labService;
            _classRepository = classRepository;
            _userService = userService;
        }

        // --- Subject Endpoints ---

        [HttpGet("subjects")]
        public async Task<IActionResult> GetAllSubjects()
        {
            var result = await _subjectService.GetAllSubjects();
            return StatusCode(result.Code, result);
        }

        [HttpGet("subjects/{id}")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            var subject = await _subjectService.GetSubjectById(id);
            if (subject != null)
            {
                return Ok(new {
                    success = true,
                    message = "Lấy chi tiết môn học thành công!",
                    code = 200,
                    data = new {
                        subjectName = subject.SubjectName,
                        subjectDescription = subject.SubjectDescription,
                        subjectStatus = subject.SubjectStatus
                    }
                });
            }
            else
            {
                return NotFound(new {
                    success = false,
                    message = "Không tìm thấy môn học!",
                    code = 404,
                    data = (object)null
                });
            }
        }

        [HttpPost("subjects")]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequestModel subjectModel)
        {
            try
            {
                await _subjectService.AddSubject(subjectModel);
                return Ok(new {
                    success = true,
                    message = "Tạo môn học thành công!",
                    data = subjectModel
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

        [HttpPut("subjects/{id}")]
        public async Task<IActionResult> UpdateSubject(int id, [FromBody] UpdateSubjectRequestModel subjectModel)
        {
            try
            {
                await _subjectService.UpdateSubject(id, subjectModel);
                return Ok(new {
                    success = true,
                    message = "Cập nhật môn học thành công!",
                    data = subjectModel
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

        [HttpDelete("subjects/{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            try
            {
                await _subjectService.DeleteSubject(id);
                return Ok(new {
                    success = true,
                    message = "Xóa môn học thành công!",
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

        // --- Lab Endpoints ---

        [HttpGet("subjects/{subjectId}/labs")]
        public async Task<IActionResult> GetLabsForSubject(int subjectId)
        {
            var result = await _labService.GetLabsBySubjectId(subjectId);
            return StatusCode(result.Code, result);
        }

        [HttpGet("labs/{id}")]
        public async Task<IActionResult> GetLabById(int id)
        {
            var lab = await _labService.GetLabById(id);
            if (lab != null)
            {
                return Ok(new {
                    success = true,
                    message = "Lấy chi tiết lab thành công!",
                    code = 200,
                    data = new {
                        labName = lab.LabName,
                        labRequest = lab.LabRequest,
                        labTarget = lab.LabTarget,
                        labStatus = lab.LabStatus
                    }
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

        [HttpPost("labs")]
        public async Task<IActionResult> CreateLab([FromBody] CreateLabRequestModel labModel)
        {
            try
            {
                // ✅ SỬA: Truyền đúng tham số theo signature mới
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
                    message = "Tạo bài lab thành công!",
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

        [HttpPut("labs/{id}")]
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

        [HttpDelete("labs/{id}")]
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

        [HttpGet("labs")]
        public async Task<IActionResult> GetAllLabs()
        {
            var result = await _labService.GetAllLabs();
            return StatusCode(result.Code, result);
        }

       
        }
    }
 