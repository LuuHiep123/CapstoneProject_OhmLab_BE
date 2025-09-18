using BusinessLayer.RequestModel.Kit;
using BusinessLayer.RequestModel.TeamEquipment;
using BusinessLayer.RequestModel.TeamKit;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/TeamKit/")]
    [ApiController]
    public class TeamKitController : ControllerBase
    {
        private readonly ITeamKitService _service;

        public TeamKitController(ITeamKitService services)
        {
            _service = services;
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPost("Search")]
        public async Task<IActionResult> GetAllTeamKit(GetAllTeamKitRequestModel model)
        {
            try
            {
                var result = await _service.GetListTeamKit(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPost("BorrowKit")]
        public async Task<IActionResult> CreateTeamKit(CreateTeamKitRequestModel model)
        {
            try
            {
                var result = await _service.CreateTeamKit(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPost("GivebackKit")]
        public async Task<IActionResult> GiveBackKit(int teamKit)
        {
            try
            {
                var result = await _service.FillBorrowDateForTeamKit(teamKit);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamKitById(int id)
        {
            try
            {
                var result = await _service.GetTeamKitById(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpGet("ListTeamKitTeamId/{teamId}")]
        public async Task<IActionResult> GetTeamKitByTeamId(int teamId)
        {
            try
            {
                var result = await _service.GetListTeamKitTeamId(teamId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeamKit(int id, UpdateTeamKitRequestModel model)
        {
            try
            {
                var result = await _service.UpdateTeamKit(id, model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteTeamKit(int id)
        {
            try
            {
                var result = await _service.DeleteTeamKit(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
