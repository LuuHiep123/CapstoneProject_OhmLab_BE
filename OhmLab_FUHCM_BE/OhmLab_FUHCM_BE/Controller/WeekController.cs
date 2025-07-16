using BusinessLayer.RequestModel.Week;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Week;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BusinessLayer.Service;
using System.Collections.Generic;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeekController : ControllerBase
    {
        private readonly IWeekRepository _weekRepository;
        private readonly IWeekService _weekService;
        public WeekController(IWeekRepository weekRepository, IWeekService weekService)
        {
            _weekRepository = weekRepository;
            _weekService = weekService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWeek([FromBody] CreateWeekRequestModel model)
        {
            var week = new Week
            {
                SemesterId = model.SemesterId,
                WeeksName = model.WeeksName,
                WeeksStartDate = model.WeeksStartDate,
                WeeksEndDate = model.WeeksEndDate,
                WeeksDescription = model.WeeksDescription,
                WeeksStatus = model.WeeksStatus
            };
            var result = await _weekRepository.AddAsync(week);
            var response = new WeekResponseModel
            {
                WeeksId = result.WeeksId,
                SemesterId = result.SemesterId,
                WeeksName = result.WeeksName,
                WeeksStartDate = result.WeeksStartDate,
                WeeksEndDate = result.WeeksEndDate,
                WeeksDescription = result.WeeksDescription,
                WeeksStatus = result.WeeksStatus
            };
            return Ok(new BaseResponse<WeekResponseModel>
            {
                Code = 200,
                Success = true,
                Message = "Tạo tuần học thành công!",
                Data = response
            });
        }

        [HttpGet("semester/{semesterId}")]
        public async Task<IActionResult> GetWeeksBySemester(int semesterId)
        {
            var result = await _weekService.GetBySemesterIdAsync(semesterId);
            return Ok(new BaseResponse<IEnumerable<WeekResponseModel>>
            {
                Code = 200,
                Success = true,
                Message = "Lấy danh sách tuần theo học kỳ thành công!",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeekById(int id)
        {
            var result = await _weekService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new BaseResponse<object> { Code = 404, Success = false, Message = "Không tìm thấy tuần!", Data = null });
            return Ok(new BaseResponse<WeekResponseModel>
            {
                Code = 200,
                Success = true,
                Message = "Lấy tuần thành công!",
                Data = result
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWeek(int id, [FromBody] UpdateWeekRequestModel model)
        {
            var result = await _weekService.UpdateAsync(id, model);
            if (result == null)
                return NotFound(new BaseResponse<object> { Code = 404, Success = false, Message = "Không tìm thấy tuần để cập nhật!", Data = null });
            return Ok(new BaseResponse<WeekResponseModel>
            {
                Code = 200,
                Success = true,
                Message = "Cập nhật tuần thành công!",
                Data = result
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeek(int id)
        {
            var result = await _weekService.DeleteAsync(id);
            if (!result)
                return NotFound(new BaseResponse<object> { Code = 404, Success = false, Message = "Không tìm thấy tuần để xóa!", Data = null });
            return Ok(new BaseResponse<object>
            {
                Code = 200,
                Success = true,
                Message = "Xóa tuần thành công!",
                Data = null
            });
        }
    }
} 