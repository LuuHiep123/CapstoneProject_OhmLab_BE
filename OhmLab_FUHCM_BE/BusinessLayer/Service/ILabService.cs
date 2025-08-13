using BusinessLayer.RequestModel.Lab;
using BusinessLayer.ResponseModel.Lab;
using BusinessLayer.ResponseModel.BaseResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface ILabService
    {
        Task<LabResponseModel> GetLabById(int id);
        Task<DynamicResponse<LabResponseModel>> GetLabsBySubjectId(int subjectId);
        Task<DynamicResponse<LabResponseModel>> GetLabsByLecturerId(string lecturerId);
        Task<DynamicResponse<LabResponseModel>> GetLabsByClassId(int classId);
        Task AddLab(CreateLabRequestModel lab);
        Task UpdateLab(int id, UpdateLabRequestModel lab);
        Task DeleteLab(int id);
        Task<DynamicResponse<LabResponseModel>> GetAllLabs();
        
        // Lab Equipment Management
        Task<BaseResponse<LabEquipmentResponseModel>> AddEquipmentToLab(int labId, AddLabEquipmentRequestModel model);
        Task<BaseResponse<bool>> RemoveEquipmentFromLab(int labId, string equipmentTypeId);
        Task<BaseResponse<List<LabEquipmentResponseModel>>> GetLabEquipments(int labId);
        
        // Lab Kit Management
        Task<BaseResponse<LabKitResponseModel>> AddKitToLab(int labId, AddLabKitRequestModel model);
        Task<BaseResponse<bool>> RemoveKitFromLab(int labId, string kitTemplateId);
        Task<BaseResponse<List<LabKitResponseModel>>> GetLabKits(int labId);
    }
} 