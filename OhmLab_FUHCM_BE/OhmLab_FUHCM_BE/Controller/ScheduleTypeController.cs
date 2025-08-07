using BusinessLayer.RequestModel.ScheduleType;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleTypeController : ControllerBase
    {
        private readonly IScheduleTypeService _scheduleTypeService;

        public ScheduleTypeController(IScheduleTypeService scheduleTypeService)
        {
            _scheduleTypeService = scheduleTypeService;
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost]
        public async Task<IActionResult> CreateScheduleType([FromBody] CreateScheduleTypeRequestModel model)
        {
            var result = await _scheduleTypeService.CreateScheduleTypeAsync(model);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleTypeById(int id)
        {
            var result = await _scheduleTypeService.GetScheduleTypeByIdAsync(id);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet]
        public async Task<IActionResult> GetAllScheduleTypes()
        {
            var result = await _scheduleTypeService.GetAllScheduleTypesAsync();
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableScheduleTypes()
        {
            var result = await _scheduleTypeService.GetAvailableScheduleTypesAsync();
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScheduleType(int id, [FromBody] UpdateScheduleTypeRequestModel model)
        {
            var result = await _scheduleTypeService.UpdateScheduleTypeAsync(id, model);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScheduleType(int id)
        {
            var result = await _scheduleTypeService.DeleteScheduleTypeAsync(id);
            return StatusCode(result.Code, result);
        }
    }
} 