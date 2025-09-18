using BusinessLayer.RequestModel.TeamEquipment;
using BusinessLayer.RequestModel.TeamKit;
using BusinessLayer.RequestModel.User;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamEquipmentController : ControllerBase
    {
        private readonly ITeamEquipmentService _service;

        public TeamEquipmentController(ITeamEquipmentService services)
        {
            _service = services;
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPost("Search")]
        public async Task<IActionResult> GetAllTeamEquipment(GetAllTeamEquipmentRequestModel model)
        {
            try
            {
                var result = await _service.GetListTeamEquipment(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPost("BorrowEquipment")]
        public async Task<IActionResult> CreateTeamEquipment(CreateTeamEquipmentRequestModel model)
        {
            try
            {
                var result = await _service.CreateTeamEquipment(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPost("GivebackEquipment")]
        public async Task<IActionResult> GiveBackEquipment(int teamEquipment)
        {
            try
            {
                var result = await _service.FillBorrowDateForTeamEquipment(teamEquipment);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamEquipmentById(int id)
        {
            try
            {
                var result = await _service.GetTeamEquipmentById(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpGet("ListTeamEquipmentByTeamId/{teamId}")]
        public async Task<IActionResult> GetTeamEquipmentByTeamId(int teamId)
        {
            try
            {
                var result = await _service.GetListTeamEquipmentByTeamId(teamId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpGet("ListTeamEquipmentByEquipmentId/{equipmentId}")]
        public async Task<IActionResult> GetTeamEquipmentByEquipmentId(string equipmentId)
        {
            try
            {
                var result = await _service.GetListTeamEquipmentByEquipmentId(equipmentId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeamEquipment(int id, UpdateTeamEquipmentRequestModel model)
        {
            try
            {
                var result = await _service.UpdateTeamEquipment(id, model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteTeamEquipment(int id)
        {
            try
            {
                var result = await _service.DeleteTeamEquipment(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
