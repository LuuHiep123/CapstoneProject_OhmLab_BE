using BusinessLayer.RequestModel.Slot;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost]
        public async Task<IActionResult> CreateSlot([FromBody] CreateSlotRequestModel model)
        {
            var result = await _slotService.CreateSlotAsync(model);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSlotById(int id)
        {
            var result = await _slotService.GetSlotByIdAsync(id);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment,Lecturer,Student")]
        [HttpGet]
        public async Task<IActionResult> GetAllSlots()
        {
            var result = await _slotService.GetAllSlotsAsync();
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSlot(int id, [FromBody] CreateSlotRequestModel model)
        {
            var result = await _slotService.UpdateSlotAsync(id, model);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlot(int id)
        {
            var result = await _slotService.DeleteSlotAsync(id);
            return StatusCode(result.Code, result);
        }
    }
} 