using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Team;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface ITeamUserService
    {
        Task<BaseResponse<TeamUserResponseModel>> AddUserToTeamAsync(int teamId, Guid userId);
        Task<BaseResponse<TeamUserResponseModel>> GetTeamUserByIdAsync(int id);
        Task<BaseResponse<List<TeamUserResponseModel>>> GetTeamUsersByTeamIdAsync(int teamId);
        Task<BaseResponse<List<TeamUserResponseModel>>> GetTeamUsersByUserIdAsync(Guid userId);
        Task<BaseResponse<bool>> RemoveUserFromTeamAsync(int teamUserId);
        Task<BaseResponse<bool>> IsUserInTeamAsync(Guid userId, int teamId);
        Task<BaseResponse<bool>> IsUserInClassAsync(Guid userId, int classId);
    }
} 