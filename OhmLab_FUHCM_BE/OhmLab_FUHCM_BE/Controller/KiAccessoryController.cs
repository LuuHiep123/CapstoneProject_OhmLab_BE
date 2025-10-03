using BusinessLayer.RequestModel.AccessoryKitTemplate;
using BusinessLayer.RequestModel.KitAccessory;
using BusinessLayer.RequestModel.KitTemplate;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/KitAccessory/")]
    [ApiController]
    public class KiAccessoryController : ControllerBase
    {
        private readonly IKitAccessoryService _service;

        public KiAccessoryController(IKitAccessoryService services)
        {
            _service = services;
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpPost("Search")]
        public async Task<IActionResult> GetAllKitAccessory(GetALlKitAccessoryRequestModel model)
        {
            try
            {
                var result = await _service.GetAllKitAccessory(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }


        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet("Kit/{kitId}")]
        public async Task<IActionResult> GetAllKitAccessoryByKitId(string kitId)
        {
            try
            {
                var result = await _service.GetAllKitAccessoryByKitId(kitId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKitAccessoryById(int id)
        {
            try
            {
                var result = await _service.GetKitAccessoryById(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("KitAccessory")]
        public async Task<IActionResult> CreateKitAccessory(CreateKitAccessoryRequestModel model)
        {
            try
            {
                var result = await _service.CreateKitAccessory(model);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKitAccessory(int id, UpdateKitAccessoryRequestModel model)
        {
            try
            {
                var result = await _service.UpdateKitAccessory(id, model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteKitAccessory(int id)
        {
            try
            {
                var result = await _service.DeleteKitAccessory(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }


}
