using BusinessLayer.RequestModel.Team;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Team;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface ITeamService
    {
        Task<BaseResponse<TeamResponseModel>> CreateTeamAsync(CreateTeamRequestModel model);
        Task<BaseResponse<TeamResponseModel>> GetTeamByIdAsync(int id);
        Task<BaseResponse<List<TeamResponseModel>>> GetAllTeamsAsync();
        Task<BaseResponse<List<TeamResponseModel>>> GetTeamsByClassIdAsync(int classId);
        Task<BaseResponse<TeamResponseModel>> UpdateTeamAsync(int id, UpdateTeamRequestModel model);
        Task<BaseResponse<bool>> DeleteTeamAsync(int id);
        Task<BaseResponse<List<TeamResponseModel>>> GetTeamsByLecturerIdAsync(Guid lecturerId);
    }
}