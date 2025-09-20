using BusinessLayer.RequestModel.Kit;
using BusinessLayer.RequestModel.RegistrationSchedule;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/RegistrationSchedule/")]
    [ApiController]
    public class RegistrationScheduleController : ControllerBase
    {
        private readonly IRegistrationScheduleService _service;

        public RegistrationScheduleController(IRegistrationScheduleService sregistrationScheduleService)
        {
            _service = sregistrationScheduleService;
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPost("RegistrationSchedule")]
        public async Task<IActionResult> CreateRegistrationSchedule(CreateRegistrationScheduleRequestModel model)
        {
            try
            {
                var result = await _service.CreateRegistrationSchedule(model);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Search")]
        public async Task<IActionResult> GetAllRegistrationSchedule(GetAllRegistrationScheduleRequestModel model)
        {
            try
            {
                var result = await _service.GetAllRegistrationSchedule(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpGet("ListRegistrationScheduleByTeacherId/{teacherId}")]
        public async Task<IActionResult> GetAllRegistrationScheduleByTeacherId(Guid teacherId)
        {
            try
            {
                var result = await _service.GetAllRegistrationScheduleByTeacherId(teacherId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet("RegistrationSchedule/{id}")]
        public async Task<IActionResult> GetRegistrationScheduleById(int id)
        {
            try
            {
                var result = await _service.GetRegistrationScheduleById(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegistrationSchedule(int id, UpdateRegistrationScheduleRequestModel model)
        {
            try
            {
                var result = await _service.UpdateRegistrationSchedule(id, model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteRegistrationSchedule(int id)
        {
            try
            {
                var result = await _service.DeleteRegistrationSchedule(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Accept")]
        public async Task<IActionResult> AcceptRegistrationSchedule(AcceptRejectRegistrationScheduleRequestModel model)
        {
            try
            {
                var result = await _service.AcceptRegistrtionSchedule(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Reject")]
        public async Task<IActionResult> RejectRegistrationSchedule(AcceptRejectRegistrationScheduleRequestModel model)
        {
            try
            {
                var result = await _service.RejectRegistrtionSchedule(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("CheckDupplicate")]
        public async Task<IActionResult> CheckDupplicateRegistrationSchedule(CheckDupplicateRegitrationScheduleRequestModel model)
        {
            try
            {
                var result = await _service.CheckDupplicateRegistrtionSchedule(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize]
        [HttpPost("ListSlotEmptyByDate/{date}")]
        public async Task<IActionResult> GetListSlotEmptyByDate(DateTime date)
        {
            try
            {
                var result = await _service.GetSlotEmptyByDate(date);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
