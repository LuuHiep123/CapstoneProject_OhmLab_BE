using BusinessLayer.RequestModel.Equipment;
using BusinessLayer.RequestModel.Kit;
using BusinessLayer.RequestModel.KitTemplate;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/KitTemplate/")]
    [ApiController]
    public class KitTemplateController : ControllerBase
    {
        private readonly IKitTemplateService _service;

        public KitTemplateController(IKitTemplateService services)
        {
            _service = services;
        }


        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("KitTempalte")]
        public async Task<IActionResult> CreateKitTemplate(CreateKitTemplateRequestModel model)
        {
            try
            {
                var result = await _service.CreateKitTemplate(model);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Search")]
        public async Task<IActionResult> GetAllKitTemplate(GetAllKitTemplateRequestModel model)
        {
            try
            {
                var result = await _service.GetAllKitTemplate(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKitTemplateById(string id)
        {
            try
            {
                var result = await _service.GetKitTemplateById(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKitTemplate(string id, UpdateKitTemplateRequestModel model)
        {
            try
            {
                var result = await _service.UpdateKitTemplate(id, model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteKitTemplate(string id)
        {
            try
            {
                var result = await _service.DeleteKitTemplate(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
