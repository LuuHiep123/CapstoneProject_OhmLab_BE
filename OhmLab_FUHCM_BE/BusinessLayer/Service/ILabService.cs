using BusinessLayer.RequestModel.Lab;
using BusinessLayer.ResponseModel.Lab;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface ILabService
    {
        Task<LabResponseModel> GetLabById(int id);
        Task<BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>> GetLabsBySubjectId(int subjectId);
        Task AddLab(CreateLabRequestModel lab);
        Task UpdateLab(int id, UpdateLabRequestModel lab);
        Task DeleteLab(int id);
        Task<BusinessLayer.ResponseModel.BaseResponse.DynamicResponse<LabResponseModel>> GetAllLabs();
    }
} 