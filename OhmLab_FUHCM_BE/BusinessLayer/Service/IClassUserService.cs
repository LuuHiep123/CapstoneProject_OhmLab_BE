using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IClassUserService
    {
        Task<BaseResponse<ClassUserResponseModel>> AddUserToClassAsync(Guid userId, int classId);
        Task<BaseResponse<ClassUserResponseModel>> GetClassUserByIdAsync(int id);
        Task<BaseResponse<List<ClassUserResponseModel>>> GetClassUsersByClassIdAsync(int classId);
        Task<BaseResponse<List<ClassUserResponseModel>>> GetClassUsersByUserIdAsync(Guid userId);
        Task<BaseResponse<bool>> RemoveUserFromClassAsync(Guid userId, int classId);
        Task<BaseResponse<bool>> IsUserInClassAsync(Guid userId, int classId);
    }
} 