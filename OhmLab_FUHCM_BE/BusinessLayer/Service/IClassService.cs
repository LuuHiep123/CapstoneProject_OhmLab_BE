using BusinessLayer.RequestModel.Class;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Class;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IClassService
    {
        Task<BaseResponse<ClassResponseModel>> CreateClassAsync(CreateClassRequestModel model);
        Task<BaseResponse<ClassResponseModel>> GetClassByIdAsync(int id);
        Task<BaseResponse<List<ClassResponseModel>>> GetAllClassesAsync();
        Task<BaseResponse<List<ClassResponseModel>>> GetClassesByLecturerIdAsync(Guid lecturerId);
        Task<BaseResponse<ClassResponseModel>> UpdateClassAsync(int id, UpdateClassRequestModel model);
        Task<BaseResponse<bool>> DeleteClassAsync(int id);
        Task<BaseResponse<bool>> AddScheduleForClassAsync(AddScheduleForClassRequestModel model);
    }
} 