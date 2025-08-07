using BusinessLayer.RequestModel.Equipment;
using BusinessLayer.RequestModel.EquipmentType;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/EquipmentType")]
    [ApiController]
    public class EquipmentTypeController : ControllerBase
    {
        private readonly IEquipmentTypeService _service;

        public EquipmentTypeController(IEquipmentTypeService services)
        {
            _service = services;
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("EquipmentType")]
        public async Task<IActionResult> CreateEquipmentType(CreateEquipmentTypeRequestModel model)
        {
            try
            {
                var result = await _service.CreateEquipmentType(model);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Search")]
        public async Task<IActionResult> GetAllEquipmentType(GetAllEquipmentTypeRequestModel model)
        {
            try
            {
                var result = await _service.GetAllEquipmentType(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEquipmentTypeById(string id)
        {
            try
            {
                var result = await _service.GetEquipmentTypeById(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEquipmentType(string id, UpdateEquipmentTypeRequestModel model)
        {
            try
            {
                var result = await _service.UpdateEquipmentType(id, model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteEquipmentType(string id)
        {
            try
            {
                var result = await _service.DeleteEquipmentType(id);
                return StatusCode(result.Code, result);
            }
                catch (Exception ex)
                {
                throw new Exception(ex.Message);
            }
        }

    }
}
