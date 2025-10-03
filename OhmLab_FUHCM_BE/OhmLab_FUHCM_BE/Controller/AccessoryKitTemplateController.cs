using BusinessLayer.RequestModel.Accessory;
using BusinessLayer.RequestModel.AccessoryKitTemplate;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/AccessoryKitTemplate/")]
    [ApiController]
    public class AccessoryKitTemplateController : ControllerBase
    {
        private readonly IAccessoryKitTemplateService _service;

        public AccessoryKitTemplateController(IAccessoryKitTemplateService services)
        {
            _service = services;
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPost("Search")]
        public async Task<IActionResult> GetAllAccessoryKitTemplate(GetAllAccessoryKitTemplateRequestModel model)
        {
            try
            {
                var result = await _service.GetAllAccessoryKitTemplate(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet("KitTemplate/{kitTemplateId}")]
        public async Task<IActionResult> GetAllAccessoryKitTemplateByKitTemplateId(string kitTemplateId)
        {
            try
            {
                var result = await _service.GetAllAccessoryKitTemplateByKitTemplateId(kitTemplateId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccessoryKitTemplateById(int id)
        {
            try
            {
                var result = await _service.GetAccessoryKitTemplateById(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("AccessoryKitTemplate")]
        public async Task<IActionResult> CreateAccessoryKitTemplate(CreateAccessoryKitTemplateRequestModel model)
        {
            try
            {
                var result = await _service.CreateAccessoryKitTemplate(model);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccessoryKitTemplate(int id, UpdateAccessoryKitTemplateRequestModel model)
        {
            try
            {
                var result = await _service.UpdateAccessoryKitTemplate(id, model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteAccessoryKitTemplate(int id)
        {
            try
            {
                var result = await _service.DeleteAccessoryKitTemplate(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
