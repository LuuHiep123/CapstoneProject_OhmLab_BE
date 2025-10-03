using BusinessLayer.RequestModel.Accessory;
using BusinessLayer.RequestModel.AccessoryKitTemplate;
using BusinessLayer.ResponseModel.Accessory;
using BusinessLayer.ResponseModel.AccessoryKitTemplate;
using BusinessLayer.ResponseModel.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IAccessoryKitTemplateService
    {
        Task<BaseResponse<AccessoryKitTemplateResponseModel>> CreateAccessoryKitTemplate(CreateAccessoryKitTemplateRequestModel model);
        Task<BaseResponse<AccessoryKitTemplateResponseModel>> DeleteAccessoryKitTemplate(int id);
        Task<BaseResponse<AccessoryKitTemplateResponseModel>> UpdateAccessoryKitTemplate(int id, UpdateAccessoryKitTemplateRequestModel model);
        Task<DynamicResponse<AccessoryKitTemplateResponseModel>> GetAllAccessoryKitTemplate(GetAllAccessoryKitTemplateRequestModel model);
        Task<BaseResponse<List<AccessoryKitTemplateResponseModel>>> GetAllAccessoryKitTemplateByKitTemplateId(string kitTemplateId);
        Task<BaseResponse<AccessoryKitTemplateResponseModel>> GetAccessoryKitTemplateById(int id);
    }
}
