using BusinessLayer.RequestModel.TeamEquipment;
using BusinessLayer.RequestModel.User;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.TeamEquipment;
using BusinessLayer.ResponseModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface ITeamEquipmentService
    {
        Task<DynamicResponse<TeamEquipmentAllResponseModel>> GetListTeamEquipment(GetAllTeamEquipmentRequestModel model);
        Task<BaseResponse<TeamEquipmentAllResponseModel>> CreateTeamEquipment(CreateTeamEquipmentRequestModel model);
        Task<BaseResponse<TeamEquipmentAllResponseModel>> GetTeamEquipmentById(int id);
        Task<BaseResponse<List<TeamEquipmentAllResponseModel>>> GetListTeamEquipmentByTeamId(int teamId);
        Task<BaseResponse<List<TeamEquipmentAllResponseModel>>> GetListTeamEquipmentByEquipmentId(string equipmentId);
        Task<BaseResponse<TeamEquipmentAllResponseModel>> FillBorrowDateForTeamEquipment(int teamEquipmentId);

    }
}
