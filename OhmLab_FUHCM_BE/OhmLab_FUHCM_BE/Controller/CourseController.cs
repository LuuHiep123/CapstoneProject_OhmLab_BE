using BusinessLayer.RequestModel.Lab;
using BusinessLayer.RequestModel.Subject;
using BusinessLayer.Service;
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

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly ILabService _labService;
        private readonly IClassRepository _classRepository;

        public CourseController(ISubjectService subjectService, ILabService labService, IClassRepository classRepository)
        {
            _subjectService = subjectService;
            _labService = labService;
            _classRepository = classRepository;
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

        [HttpGet("lecturers/{lecturerId}/labs")]
        public async Task<IActionResult> GetLabsForLecturer(string lecturerId)
        {
            var result = await _labService.GetLabsByLecturerId(lecturerId);
            return StatusCode(result.Code, result);
        }

        [HttpGet("classes/{classId}/labs")]
        public async Task<IActionResult> GetLabsForClass(int classId)
        {
            var result = await _labService.GetLabsByClassId(classId);
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
                await _labService.AddLab(labModel);
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

        // --- Lab Equipment Management ---

        [HttpPost("labs/{labId}/equipment")]
        public async Task<IActionResult> AddEquipmentToLab(int labId, [FromBody] AddLabEquipmentRequestModel model)
        {
            var result = await _labService.AddEquipmentToLab(labId, model);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("labs/{labId}/equipment/{equipmentTypeId}")]
        public async Task<IActionResult> RemoveEquipmentFromLab(int labId, string equipmentTypeId)
        {
            var result = await _labService.RemoveEquipmentFromLab(labId, equipmentTypeId);
            return StatusCode(result.Code, result);
        }

        [HttpGet("labs/{labId}/equipment")]
        public async Task<IActionResult> GetLabEquipments(int labId)
        {
            var result = await _labService.GetLabEquipments(labId);
            return StatusCode(result.Code, result);
        }

        // --- Lab Kit Management ---

        [HttpPost("labs/{labId}/kit")]
        public async Task<IActionResult> AddKitToLab(int labId, [FromBody] AddLabKitRequestModel model)
        {
            var result = await _labService.AddKitToLab(labId, model);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("labs/{labId}/kit/{kitTemplateId}")]
        public async Task<IActionResult> RemoveKitFromLab(int labId, string kitTemplateId)
        {
            var result = await _labService.RemoveKitFromLab(labId, kitTemplateId);
            return StatusCode(result.Code, result);
        }

        [HttpGet("labs/{labId}/kit")]
        public async Task<IActionResult> GetLabKits(int labId)
        {
            var result = await _labService.GetLabKits(labId);
            return StatusCode(result.Code, result);
        }
    }
}
 