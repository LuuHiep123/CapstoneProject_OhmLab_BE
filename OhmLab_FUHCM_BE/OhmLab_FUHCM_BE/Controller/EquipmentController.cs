using BusinessLayer.RequestModel.Equipment;
using BusinessLayer.RequestModel.User;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/Equipment")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _service;

        public EquipmentController(IEquipmentService services)
        {
            _service = services;
        }
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Equipment")]
        public async Task<IActionResult> CreateEquipment(CreateEquipmentRequestModel model)
        {
            try
            {
                var result = await _service.CreateEquipment(model);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("QR")]
        public async Task<IActionResult> CreateQR(string id, string QR)
        {
            try
            {
                var result = await _service.AddQR(id,QR);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Search")]
        public async Task<IActionResult> GetAllEquipment(GetAllEquipmentRequestModel model)
        {
            try
            {
                var result = await _service.GetAllEquipment(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEquipmentById(string id)
        {
            try
            {
                var result = await _service.GetEquipmentById(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEquipment(string id, UpdateEquipmentRequestModel model)
        {
            try
            {
                var result = await _service.UpdateEquipment(id, model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteEquipment(string id)
        {
            try
            {
                var result = await _service.DeleteEquipment(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
