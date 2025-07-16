using BusinessLayer.RequestModel.Semester;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Semester;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _semesterService;
        public SemesterController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSemester([FromBody] CreateSemesterRequestModel model)
        {
            var result = await _semesterService.CreateSemesterAsync(model);
            return Ok(new BaseResponse<SemesterResponseModel>
            {
                Code = 200,
                Success = true,
                Message = "Tạo học kỳ thành công!",
                Data = result
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSemesters()
        {
            var result = await _semesterService.GetAllAsync();
            return Ok(new BaseResponse<IEnumerable<SemesterResponseModel>>
            {
                Code = 200,
                Success = true,
                Message = "Lấy danh sách học kỳ thành công!",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSemesterById(int id)
        {
            var result = await _semesterService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new BaseResponse<object> { Code = 404, Success = false, Message = "Không tìm thấy học kỳ!", Data = null });
            return Ok(new BaseResponse<SemesterResponseModel>
            {
                Code = 200,
                Success = true,
                Message = "Lấy học kỳ thành công!",
                Data = result
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSemester(int id, [FromBody] UpdateSemesterRequestModel model)
        {
            var result = await _semesterService.UpdateAsync(id, model);
            if (result == null)
                return NotFound(new BaseResponse<object> { Code = 404, Success = false, Message = "Không tìm thấy học kỳ để cập nhật!", Data = null });
            return Ok(new BaseResponse<SemesterResponseModel>
            {
                Code = 200,
                Success = true,
                Message = "Cập nhật học kỳ thành công!",
                Data = result
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemester(int id)
        {
            var result = await _semesterService.DeleteAsync(id);
            if (!result)
                return NotFound(new BaseResponse<object> { Code = 404, Success = false, Message = "Không tìm thấy học kỳ để xóa!", Data = null });
            return Ok(new BaseResponse<object>
            {
                Code = 200,
                Success = true,
                Message = "Xóa học kỳ thành công!",
                Data = null
            });
        }
    }
} 