
using BusinessLayer.RequestModel.User;
using BusinessLayer.ResponseModel.User;
using BusinessLayer.ResponseModel;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Claims;
using X.PagedList;
using BusinessLayer.ResponseModel.BaseResponse;

namespace SWDProject_BE.Controllers
{
	[Route("API/User/")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _service;

		public UserController(IUserService services)
		{
            _service = services;
		}

        [HttpPost("LoginTest")]
        public async Task<IActionResult> LoginTest(string email)
        {
            try
            {
                var result = await _service.LoginTest(email);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost("Admin")]
        public async Task<IActionResult> RegisterAdmin(string email, string name)
        {
            try
            {
                var result = await _service.CreateAccountAdmin(email, name);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("HeadOfDepartment")]
        public async Task<IActionResult> RegisterHeadOfDepartment(string email, string name)
        {
            try
            {
                var result = await _service.CreateAccountHeadOfDepartment(email, name);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Student")]
        public async Task<IActionResult> Register(RegisterRequestModel model)
        {
            try
            {
                var result = await _service.RegisterUser(model);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("block/{id}")]
        public async Task<IActionResult> BlockUser(Guid id)
        {
            try
            {
                var result = await _service.BlockUser(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("LoginMail")]
        public async Task<IActionResult> LoginMail(LoginMailModel model)
        {
            try
            {
                var result = await _service.LoginMail(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPost("Search")]
        public async Task<IActionResult> GetAllUser(GetAllUserRequestModel model)
        {
            try
            {
                var result = await _service.GetListUser(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var result = await _service.GetUserById(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin,HeadOfDepartment")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateRequestModel model)
        {
            try
            {
                var result = await _service.UpdateUser(id, model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var result = await _service.DeleteUser(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("Current-User")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return StatusCode(401, new BaseResponse()
                    {
                        Code = 401,
                        Success = false,
                        Message = "User information not found!."
                    });
                }
                else
                {
                    var result = await _service.GetUserById(Guid.Parse(userId));
                    return StatusCode(result.Code, result);
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }   
    }
}
