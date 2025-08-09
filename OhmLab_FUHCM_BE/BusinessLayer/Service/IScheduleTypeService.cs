using BusinessLayer.RequestModel.Assignment;
using BusinessLayer.RequestModel.ScheduleType;
using BusinessLayer.ResponseModel.Assignment;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.ScheduleType;
using DataLayer.Entities;

namespace BusinessLayer.Service
{
    public interface IScheduleTypeService
    {
        Task<BaseResponse<ScheduleTypeResponseModel>> CreateScheduleTypeAsync(CreateScheduleTypeRequestModel model);
        Task<BaseResponse<ScheduleTypeResponseModel>> GetScheduleTypeByIdAsync(int id);
        Task<BaseResponse<List<ScheduleTypeResponseModel>>> GetAllScheduleTypesAsync();
        Task<BaseResponse<ScheduleTypeResponseModel>> UpdateScheduleTypeAsync(int id, UpdateScheduleTypeRequestModel model);
        Task<BaseResponse<bool>> DeleteScheduleTypeAsync(int id);
        Task<BaseResponse<List<ScheduleTypeResponseModel>>> GetAvailableScheduleTypesAsync();
    }
} 