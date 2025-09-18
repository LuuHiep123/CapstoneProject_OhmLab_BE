using BusinessLayer.RequestModel.TeamEquipment;
using BusinessLayer.RequestModel.TeamKit;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.TeamEquipment;
using BusinessLayer.ResponseModel.TeamKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface ITeamKitService
    {
        Task<DynamicResponse<TeamKitAllResponseModel>> GetListTeamKit(GetAllTeamKitRequestModel model);
        Task<BaseResponse<TeamKitAllResponseModel>> CreateTeamKit(CreateTeamKitRequestModel model);
        Task<BaseResponse<TeamKitAllResponseModel>> GetTeamKitById(int id);
        Task<BaseResponse<List<TeamKitAllResponseModel>>> GetListTeamKitTeamId(int teamId);
        Task<BaseResponse<List<TeamKitAllResponseModel>>> GetListTeamKitByKitId(string kitId);
        Task<BaseResponse<TeamKitAllResponseModel>> FillBorrowDateForTeamKit(int teamKitId);
        Task<BaseResponse<TeamKitAllResponseModel>> UpdateTeamKit(int teamKitId, UpdateTeamKitRequestModel model);
        Task<BaseResponse<TeamKitAllResponseModel>> DeleteTeamKit(int teamKitId);
    }
}
