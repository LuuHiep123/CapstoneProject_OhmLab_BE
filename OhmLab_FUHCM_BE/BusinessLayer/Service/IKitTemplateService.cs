using BusinessLayer.RequestModel.Equipment;
using BusinessLayer.RequestModel.Kit;
using BusinessLayer.RequestModel.KitTemplate;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Kit;
using BusinessLayer.ResponseModel.KitTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IKitTemplateService
    {
        Task<BaseResponse<KitTemplateResponseModel>> CreateKitTemplate (CreateKitTemplateRequestModel model);
        Task<BaseResponse<KitTemplateResponseModel>> DeleteKitTemplate(string id);
        Task<BaseResponse<KitTemplateResponseModel>> UpdateKitTemplate(string id, UpdateKitTemplateRequestModel model);
        Task<DynamicResponse<KitTemplateResponseModel>> GetAllKitTemplate(GetAllKitTemplateRequestModel model);
        Task<BaseResponse<KitTemplateResponseModel>> GetKitTemplateById(string id);

    }
}
