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

    }
}
