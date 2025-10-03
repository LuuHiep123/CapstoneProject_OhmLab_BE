using BusinessLayer.RequestModel.Accessory;
using BusinessLayer.RequestModel.Kit;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/Accessory/")]
    [ApiController]
    public class AccessoryController : ControllerBase
    {
        private readonly IAccessoryService _service;

        public AccessoryController(IAccessoryService services)
        {
            _service = services;
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPost("Search")]
        public async Task<IActionResult> GetAllAccessory(GetAllAccessoryRequestModel model)
        {
            try
            {
                var result = await _service.GetAllAccessory(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccessoryById(int id)
        {
            try
            {
                var result = await _service.GetAccessoryById(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Accessory")]
        public async Task<IActionResult> CreateAccessory(CreateAccessoryRequestModel model)
        {
            try
            {
                var result = await _service.CreateAccessory(model);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccessory(int id, UpdateAccessoryRequestModel model)
        {
            try
            {
                var result = await _service.UpdateAccessory(id, model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteAccessory(int id)
        {
            try
            {
                var result = await _service.DeleteAccessory(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
