using BusinessLayer.RequestModel.Equipment;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Equipment;
using BusinessLayer.ResponseModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IEquipmentService
    {
        Task<BaseResponse<EquipmentResponseModel>> CreateEquipment(CreateEquipmentRequestModel model);
        Task<BaseResponse<EquipmentResponseModel>> AddQR(string id, string UrlQR);
        Task<BaseResponse<EquipmentResponseModel>> GetEquipmentById(string id);
        Task<BaseResponse<List<EquipmentResponseModel>>> GetEquipmentByEquipmentTypeId(string equipmentType);
        Task<DynamicResponse<EquipmentResponseModel>> GetAllEquipment(GetAllEquipmentRequestModel model);
        Task<BaseResponse<EquipmentResponseModel>> DeleteEquipment(string id);
        Task<BaseResponse<EquipmentResponseModel>> UpdateEquipment(string id,UpdateEquipmentRequestModel model);
    }
}
