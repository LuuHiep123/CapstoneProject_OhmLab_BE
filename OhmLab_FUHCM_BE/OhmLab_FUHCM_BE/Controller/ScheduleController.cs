using BusinessLayer.RequestModel.Assignment;
using BusinessLayer.RequestModel.Class;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/Schedule/")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly IClassService _classService;

        public ScheduleController(IClassService classService, IScheduleService scheduleService)
        {
            _classService = classService;
            _scheduleService = scheduleService;
        }


        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet()]
        public async Task<IActionResult> GetAllSchedule()
        {
            try
            {
                var result = await _scheduleService.GetAllScheduleAsync();
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet("Student/{StudentId}")]
        public async Task<IActionResult> GetScheduleByStudentId(Guid StudentId)
        {
            try
            {
                var result = await _scheduleService.GetAllScheduleByStudentIdAsync(StudentId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet("Lecture/{LectureId}")]
        public async Task<IActionResult> GetScheduleByLectureId(Guid LectureId)
        {
            try
            {
                var result = await _scheduleService.GetAllScheduleByLectureIdAsync(LectureId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("AddScheduleForClass")]
        public async Task<IActionResult> AddScheduleForClass(AddScheduleForClassRequestModel model)
        {
            try
            {
                var result = await _classService.AddScheduleForClassAsync(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
