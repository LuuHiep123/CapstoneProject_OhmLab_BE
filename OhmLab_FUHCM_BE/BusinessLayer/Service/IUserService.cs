using BusinessLayer.RequestModel.User;
using BusinessLayer.ResponseModel.User;
using BusinessLayer.ResponseModel.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IUserService
    {
        Task<BaseResponse<UserResponseModel>> CreateAccountAdmin(string email, string name);
         Task<BaseResponse<UserResponseModel>> CreateAccountHeadOfDepartment(string email, string name);
        Task<BaseResponse<LoginResponseModel>> LoginMail(LoginMailModel model);
         Task<BaseResponse<UserResponseModel>> RegisterUser(RegisterRequestModel model);
        Task<DynamicResponse<UserResponseModel>> GetListUser(GetAllUserRequestModel model);
        Task<BaseResponse> BlockUser(Guid userId);
        Task<BaseResponse<LoginResponseModel>> LoginTest(string mail);
        Task<BaseResponse<UserResponseModel>> DeleteUser(Guid id);
        Task<BaseResponse<UserResponseModel>> GetUserById(Guid id);
        Task<BaseResponse<UserResponseModel>> UpdateUser(Guid id, UpdateRequestModel model);
    }
}
