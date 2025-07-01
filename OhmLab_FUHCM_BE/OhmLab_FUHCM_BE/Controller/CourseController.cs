using BusinessLayer.RequestModel.Lab;
using BusinessLayer.RequestModel.Subject;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace OhmLab_FUHCM_BE.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly ILabService _labService;

        public CourseController(ISubjectService subjectService, ILabService labService)
        {
            _subjectService = subjectService;
            _labService = labService;
        }

        // --- Subject Endpoints ---

        [HttpGet("subjects")]
        public async Task<IActionResult> GetAllSubjects()
        {
            try
            {
                var subjects = await _subjectService.GetAllSubjects();
                return Ok(subjects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("subjects/{id}")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            try
            {
                var subject = await _subjectService.GetSubjectById(id);
                if (subject == null)
                {
                    return NotFound();
                }
                return Ok(subject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("subjects")]
        [Authorize(Roles = "HEADOFDEPARTMENT")]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequestModel subjectModel)
        {
            try
            {
                await _subjectService.AddSubject(subjectModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("subjects/{id}")]
        [Authorize(Roles = "HEADOFDEPARTMENT")]
        public async Task<IActionResult> UpdateSubject(int id, [FromBody] UpdateSubjectRequestModel subjectModel)
        {
            try
            {
                await _subjectService.UpdateSubject(id, subjectModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("subjects/{id}")]
        [Authorize(Roles = "HEADOFDEPARTMENT")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            try
            {
                await _subjectService.DeleteSubject(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // --- Lab Endpoints ---

        [HttpGet("subjects/{subjectId}/labs")]
        public async Task<IActionResult> GetLabsForSubject(int subjectId)
        {
            try
            {
                var labs = await _labService.GetLabsBySubjectId(subjectId);
                return Ok(labs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("labs/{id}")]
        public async Task<IActionResult> GetLabById(int id)
        {
            try
            {
                var lab = await _labService.GetLabById(id);
                if (lab == null)
                {
                    return NotFound();
                }
                return Ok(lab);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("labs")]
        [Authorize(Roles = " LECTURER")]
        public async Task<IActionResult> CreateLab([FromBody] CreateLabRequestModel labModel)
        {
            try
            {
                await _labService.AddLab(labModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("labs/{id}")]
        [Authorize(Roles = " LECTURER")]
        public async Task<IActionResult> UpdateLab(int id, [FromBody] UpdateLabRequestModel labModel)
        {
            try
            {
                await _labService.UpdateLab(id, labModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("labs/{id}")]
        //[Authorize(Roles = "HEADOFDEPARTMENT, LECTURER")]
        public async Task<IActionResult> DeleteLab(int id)
        {
            try
            {
                await _labService.DeleteLab(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 