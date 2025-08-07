using BusinessLayer.RequestModel.Equipment;
using BusinessLayer.RequestModel.EquipmentType;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Equipment;
using BusinessLayer.ResponseModel.EquipmentType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IEquipmentTypeService
    {
        Task<BaseResponse<EquipmentTypeResponseModel>> CreateEquipmentType(CreateEquipmentTypeRequestModel model);
        Task<BaseResponse<EquipmentTypeResponseModel>> GetEquipmentTypeById(string id);
        Task<DynamicResponse<EquipmentTypeResponseModel>> GetAllEquipmentType(GetAllEquipmentTypeRequestModel model);
        Task<BaseResponse<EquipmentTypeResponseModel>> DeleteEquipmentType(string id);
        Task<BaseResponse<EquipmentTypeResponseModel>> UpdateEquipmentType(string id, UpdateEquipmentTypeRequestModel model);
    }
}
