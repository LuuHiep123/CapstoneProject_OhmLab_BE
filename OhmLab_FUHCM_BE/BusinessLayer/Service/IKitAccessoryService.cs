using BusinessLayer.RequestModel.AccessoryKitTemplate;
using BusinessLayer.RequestModel.KitAccessory;
using BusinessLayer.ResponseModel.AccessoryKitTemplate;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.KitAccessory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IKitAccessoryService
    {
        Task<BaseResponse<KitAccessoryResponseModel>> CreateKitAccessory(CreateKitAccessoryRequestModel model);
        Task<BaseResponse<KitAccessoryResponseModel>> DeleteKitAccessory(int id);
        Task<BaseResponse<KitAccessoryResponseModel>> UpdateKitAccessory(int id, UpdateKitAccessoryRequestModel model);
        Task<DynamicResponse<KitAccessoryResponseModel>> GetAllKitAccessory(GetALlKitAccessoryRequestModel model);
        Task<BaseResponse<List<KitAccessoryResponseModel>>> GetAllKitAccessoryByKitId(string KitId);
        Task<BaseResponse<KitAccessoryResponseModel>> GetKitAccessoryById(int id);
    }
}
