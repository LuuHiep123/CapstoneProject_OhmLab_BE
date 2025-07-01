using BusinessLayer.RequestModel.User;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class EquipmentService : IEquipmentService
    {
        Task<BaseResponse<UserResponseModel>> CreateAccountAdmin(string email, string name)
        {
            throw new NotImplementedException();
        }
        Task<BaseResponse<UserResponseModel>> CreateAccountHeadOfDepartment(string email, string name)
        {
            throw new NotImplementedException();
        }
        Task<BaseResponse<LoginResponseModel>> LoginMail(LoginMailModel model)
        {
            throw new NotImplementedException();
        }
        Task<BaseResponse<UserResponseModel>> RegisterUser(RegisterRequestModel model)
        {
            throw new NotImplementedException();
        }
        Task<DynamicResponse<UserResponseModel>> GetListUser(GetAllUserRequestModel model)
        {
            throw new NotImplementedException();
        }
        Task<BaseResponse> BlockUser(Guid userId)
        {
            throw new NotImplementedException();
        }
        Task<BaseResponse<LoginResponseModel>> LoginTest(string mail)
        {
            throw new NotImplementedException();
        }
        Task<BaseResponse<UserResponseModel>> DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }
        Task<BaseResponse<UserResponseModel>> GetUserById(Guid id)
        {
            throw new NotImplementedException();
        }
        Task<BaseResponse<UserResponseModel>> UpdateUser(Guid id, UpdateRequestModel model)
        {
            throw new NotImplementedException();
        }
    }
}
