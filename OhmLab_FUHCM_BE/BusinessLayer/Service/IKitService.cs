using BusinessLayer.RequestModel.Equipment;
using BusinessLayer.RequestModel.Kit;
using BusinessLayer.RequestModel.KitTemplate;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Equipment;
using BusinessLayer.ResponseModel.Kit;
using BusinessLayer.ResponseModel.KitTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IKitService
    {
        Task<BaseResponse<KitResponseModel>> CreateKit(CreateKitRequestModel model);
        Task<BaseResponse<KitResponseModel>> DeleteKit(string id);
        Task<BaseResponse<KitResponseModel>> UpdateKit(string id, UpdateKitRequestModel model);
        Task<DynamicResponse<KitResponseModel>> GetAllKit(GetAllKitRequestModel model);
        Task<DynamicResponse<KitResponseModel>> GetAllKitByKitTempalteId(GetAllKitByKitTemplateIdRequestModel model);
        Task<BaseResponse<KitResponseModel>> GetKitById(string id);
    }
}
