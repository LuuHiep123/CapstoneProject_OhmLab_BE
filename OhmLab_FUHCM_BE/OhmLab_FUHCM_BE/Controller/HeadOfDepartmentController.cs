using BusinessLayer.RequestModel.HeadOfDepartment;
using BusinessLayer.ResponseModel.HeadOfDepartment;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/head-of-department")]
    [ApiController]
    [Authorize]
    public class HeadOfDepartmentController : ControllerBase
    {
        private readonly IHeadOfDepartmentService _hodService;

        public HeadOfDepartmentController(IHeadOfDepartmentService hodService)
        {
            _hodService = hodService;
        }

        // Dashboard Overview - Chỉ giữ lại dashboard và thống kê
        [HttpGet("dashboard")]
        [Authorize(Roles = "HeadOfDepartment")]
        public async Task<IActionResult> GetDashboardOverview()
        {
            try
            {
                var result = await _hodService.GetDashboardOverview();
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    data = (object)null
                });
            }
        }

        // Lab Monitoring - Chỉ giữ lại các API thống kê
        [HttpPost("monitoring/overview")]
        [Authorize(Roles = "HeadOfDepartment")]
        public async Task<IActionResult> GetLabMonitoringOverview([FromBody] MonitorLabRequestModel model)
        {
            try
            {
                var result = await _hodService.GetLabMonitoringOverview(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    data = (object)null
                });
            }
        }

        [HttpGet("monitoring/subject-statistics")]
        [Authorize(Roles = "HeadOfDepartment")]
        public async Task<IActionResult> GetSubjectStatistics()
        {
            try
            {
                var result = await _hodService.GetSubjectStatistics();
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    data = (object)null
                });
            }
        }

        [HttpGet("monitoring/lecturer-performance")]
        [Authorize(Roles = "HeadOfDepartment")]
        public async Task<IActionResult> GetLecturerPerformance()
        {
            try
            {
                var result = await _hodService.GetLecturerPerformance();
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    data = (object)null
                });
            }
        }

        [HttpGet("monitoring/equipment-usage")]
        [Authorize(Roles = "HeadOfDepartment")]
        public async Task<IActionResult> GetEquipmentUsage()
        {
            try
            {
                var result = await _hodService.GetEquipmentUsage();
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    data = (object)null
                });
            }
        }
    }
}
