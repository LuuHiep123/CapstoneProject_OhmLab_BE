using BusinessLayer.RequestModel.Lab;
using BusinessLayer.RequestModel.Subject;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly ILabService _labService;

        public CourseController(ISubjectService subjectService, ILabService labService)
        {
            _subjectService = subjectService;
            _labService = labService;
        }

        // --- Subject Endpoints ---

        [HttpGet("subjects")]
        public async Task<IActionResult> GetAllSubjects()
        {
            try
            {
                var subjects = await _subjectService.GetAllSubjects();
                return Ok(subjects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("subjects/{id}")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            try
            {
                var subject = await _subjectService.GetSubjectById(id);
                if (subject == null)
                {
                    return NotFound();
                }
                return Ok(subject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("subjects")]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequestModel subjectModel)
        {
            try
            {
                await _subjectService.AddSubject(subjectModel);
                return Ok(new { success = true, message = "Tạo môn học thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Tạo môn học thất bại!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpPut("subjects/{id}")]
        
        public async Task<IActionResult> UpdateSubject(int id, [FromBody] UpdateSubjectRequestModel subjectModel)
        {
            try
            {
                await _subjectService.UpdateSubject(id, subjectModel);
                return Ok(new { success = true, message = "Cập nhật môn học thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Cập nhật môn học thất bại!", error = ex.Message });
            }
        }

        [HttpDelete("subjects/{id}")]
    
        public async Task<IActionResult> DeleteSubject(int id)
        {
            try
            {
                await _subjectService.DeleteSubject(id);
                return Ok(new { success = true, message = "Xóa môn học thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Xóa môn học thất bại!", error = ex.Message });
            }
        }

        // --- Lab Endpoints ---

        [HttpGet("subjects/{subjectId}/labs")]
        public async Task<IActionResult> GetLabsForSubject(int subjectId)
        {
            try
            {
                var labs = await _labService.GetLabsBySubjectId(subjectId);
                return Ok(labs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("labs/{id}")]
        public async Task<IActionResult> GetLabById(int id)
        {
            try
            {
                var lab = await _labService.GetLabById(id);
                if (lab == null)
                {
                    return NotFound();
                }
                return Ok(lab);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("labs")]
    
        public async Task<IActionResult> CreateLab([FromBody] CreateLabRequestModel labModel)
        {
            try
            {
                await _labService.AddLab(labModel);
                return Ok(new { success = true, message = "Tạo bài lab thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Tạo bài lab  thất bại!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpPut("labs/{id}")]

        public async Task<IActionResult> UpdateLab(int id, [FromBody] UpdateLabRequestModel labModel)
        {
            try
            {
                await _labService.UpdateLab(id, labModel);
                return Ok(new { success = true, message = "Cập nhật bài lab thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Cập nhật bài lab thất bại!", error = ex.Message });
            }
        }

        [HttpDelete("labs/{id}")]
        //[Authorize(Roles = "HEADOFDEPARTMENT, LECTURER")]
        public async Task<IActionResult> DeleteLab(int id)
        {
            try
            {
                await _labService.DeleteLab(id);
                return Ok(new { success = true, message = "Xóa bài lab thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Xóa bài lab thất bại!", error = ex.Message });
            }
        }

        [HttpGet("labs")]
        public async Task<IActionResult> GetAllLabs()
        {
            try
            {
                var labs = await _labService.GetAllLabs();
                return Ok(labs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 