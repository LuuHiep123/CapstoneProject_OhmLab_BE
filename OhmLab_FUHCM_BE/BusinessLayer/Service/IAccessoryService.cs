using BusinessLayer.RequestModel.Accessory;
using BusinessLayer.ResponseModel.Accessory;
using BusinessLayer.ResponseModel.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IAccessoryService
    {
        Task<BaseResponse<AccessoryResponseModel>> CreateAccessory(CreateAccessoryRequestModel model);
        Task<BaseResponse<AccessoryResponseModel>> DeleteAccessory(int id);
        Task<BaseResponse<AccessoryResponseModel>> UpdateAccessory(int id, UpdateAccessoryRequestModel model);
        Task<DynamicResponse<AccessoryResponseModel>> GetAllAccessory(GetAllAccessoryRequestModel model);
        Task<BaseResponse<AccessoryResponseModel>> GetAccessoryById(int id);
    }
}
