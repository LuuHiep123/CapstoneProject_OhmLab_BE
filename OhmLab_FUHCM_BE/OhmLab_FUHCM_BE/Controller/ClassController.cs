using BusinessLayer.RequestModel.Class;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost]
        public async Task<IActionResult> CreateClass([FromBody] CreateClassRequestModel model)
        {
            var result = await _classService.CreateClassAsync(model);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassById(int id)
        {
            var result = await _classService.GetClassByIdAsync(id);
            return StatusCode(result.Code, result);
        }

        

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpGet]
        public async Task<IActionResult> GetAllClasses([FromQuery] string? status = null)
        {
            var requestModel = new GetAllClassRequestModel { Status = status };
            var result = await _classService.GetAllClassesAsync(requestModel);
            return StatusCode(result.Code, result);
        }


      

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpGet("lecturer/{lecturerId}")]
        public async Task<IActionResult> GetClassesByLecturerId(Guid lecturerId)
        {
            var result = await _classService.GetClassesByLecturerIdAsync(lecturerId);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Student")]
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetClassesByStudentId(Guid studentId)
        {
            var result = await _classService.GetClassesByStudentIdAsync(studentId);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClass(int id, [FromBody] UpdateClassRequestModel model)
        {
            var result = await _classService.UpdateClassAsync(id, model);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateClassStatus(int id, [FromBody] UpdateClassStatusRequestModel model)
        {
            var result = await _classService.UpdateClassStatusAsync(id, model.Status);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("AddScheduleForClass")]
        public async Task<IActionResult> AddScheduleforClass([FromBody] AddScheduleForClassRequestModel model)
        {
            var result = await _classService.AddScheduleForClassAsync(model);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpGet("{classId}/labs")]
        public async Task<IActionResult> GetLabsByClassId(int classId)
        {
            var result = await _classService.GetLabsByClassIdAsync(classId);
            return StatusCode(result.Code, result);
        }
    }
} 