using BusinessLayer.RequestModel.Equipment;
using BusinessLayer.RequestModel.Kit;
using BusinessLayer.RequestModel.KitTemplate;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/Kit/")]
    [ApiController]
    public class KitController : ControllerBase
    {
        private readonly IKitService _service;

        public KitController(IKitService services)
        {
            _service = services;
        }


        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Kit")]
        public async Task<IActionResult> CreateKit(CreateKitRequestModel model)
        {
            try
            {
                var result = await _service.CreateKit(model);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPost("Search")]
        public async Task<IActionResult> GetAllKit(GetAllKitRequestModel model)
        {
            try
            {
                var result = await _service.GetAllKit(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("SearchByKitTemplateId")]
        public async Task<IActionResult> GetAllKitByKitTemplateId(GetAllKitByKitTemplateIdRequestModel model)
        {
            try
            {
                var result = await _service.GetAllKitByKitTempalteId(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKitById(string id)
        {
            try
            {
                var result = await _service.GetKitById(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKit(string id, UpdateKitRequestModel model)
        {
            try
            {
                var result = await _service.UpdateKit(id, model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteKit(string id)
        {
            try
            {
                var result = await _service.DeleteKit(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
