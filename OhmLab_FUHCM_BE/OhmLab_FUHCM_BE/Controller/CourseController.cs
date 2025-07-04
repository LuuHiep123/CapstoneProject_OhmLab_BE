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
            var result = await _subjectService.GetAllSubjects();
            return StatusCode(result.Code, result);
        }

        [HttpGet("subjects/{id}")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            var subject = await _subjectService.GetSubjectById(id);
            var pageData = subject != null ? new List<BusinessLayer.ResponseModel.Subject.SubjectResponseModel> { subject } : new List<BusinessLayer.ResponseModel.Subject.SubjectResponseModel>();
            var totalItem = subject != null ? 1 : 0;
            var response = new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<BusinessLayer.ResponseModel.Subject.SubjectResponseModel>
            {
                Code = subject != null ? 200 : 404,
                Success = subject != null,
                Message = subject != null ? "Lấy chi tiết môn học thành công!" : "Không tìm thấy môn học!",
                Data = new BusinessLayer.ResponseModel.BaseResponse.MegaData<BusinessLayer.ResponseModel.Subject.SubjectResponseModel>
                {
                    PageData = pageData,
                    PageInfo = new BusinessLayer.ResponseModel.BaseResponse.PagingMetaData
                    {
                        Page = 1,
                        Size = 1,
                        TotalItem = totalItem,
                        TotalPage = totalItem
                    },
                    SearchInfo = null
                }
            };
            return StatusCode(response.Code, response);
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
            var pageData = lab != null ? new List<BusinessLayer.ResponseModel.Lab.LabResponseModel> { lab } : new List<BusinessLayer.ResponseModel.Lab.LabResponseModel>();
            var totalItem = lab != null ? 1 : 0;
            var response = new BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<BusinessLayer.ResponseModel.Lab.LabResponseModel>
            {
                Code = lab != null ? 200 : 404,
                Success = lab != null,
                Message = lab != null ? "Lấy chi tiết lab thành công!" : "Không tìm thấy lab!",
                Data = new BusinessLayer.ResponseModel.BaseResponse.MegaData<BusinessLayer.ResponseModel.Lab.LabResponseModel>
                {
                    PageData = pageData,
                    PageInfo = new BusinessLayer.ResponseModel.BaseResponse.PagingMetaData
                    {
                        Page = 1,
                        Size = 1,
                        TotalItem = totalItem,
                        TotalPage = totalItem
                    },
                    SearchInfo = null
                }
            };
            return StatusCode(response.Code, response);
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
    }
} 