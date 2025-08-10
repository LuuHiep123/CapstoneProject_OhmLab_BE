using BusinessLayer.RequestModel.Class;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Class;
using BusinessLayer.ResponseModel.Lab;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IClassService
    {
        Task<BaseResponse<ClassResponseModel>> CreateClassAsync(CreateClassRequestModel model);
        Task<BaseResponse<ClassResponseModel>> GetClassByIdAsync(int id);
        Task<DynamicResponse<ClassResponseModel>> GetAllClassesAsync();
        Task<DynamicResponse<ClassResponseModel>> GetAllClassesAsync(GetAllClassRequestModel model);
        Task<BaseResponse<List<ClassResponseModel>>> GetClassesByLecturerIdAsync(Guid lecturerId);
        Task<BaseResponse<ClassResponseModel>> UpdateClassAsync(int id, UpdateClassRequestModel model);
        Task<BaseResponse<bool>> DeleteClassAsync(int id);
        Task<BaseResponse<bool>> AddScheduleForClassAsync(AddScheduleForClassRequestModel model);
        Task<BaseResponse<List<LabResponseModel>>> GetLabsByClassIdAsync(int classId);
    }
} 