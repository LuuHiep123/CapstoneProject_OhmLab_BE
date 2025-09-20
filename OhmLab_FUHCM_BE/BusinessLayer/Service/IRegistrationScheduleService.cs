using BusinessLayer.RequestModel.Kit;
using BusinessLayer.RequestModel.RegistrationSchedule;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Kit;
using BusinessLayer.ResponseModel.RegistrationSchedule;
using BusinessLayer.ResponseModel.Slot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IRegistrationScheduleService
    {
        Task<BaseResponse<RegistrationScheduleAllResponseModel>> CreateRegistrationSchedule(CreateRegistrationScheduleRequestModel model);
        Task<BaseResponse<RegistrationScheduleAllResponseModel>> DeleteRegistrationSchedule(int id);
        Task<BaseResponse<RegistrationScheduleAllResponseModel>> UpdateRegistrationSchedule(int id, UpdateRegistrationScheduleRequestModel model);
        Task<DynamicResponse<RegistrationScheduleAllResponseModel>> GetAllRegistrationSchedule(GetAllRegistrationScheduleRequestModel model);
        Task<BaseResponse<List<RegistrationScheduleAllResponseModel>>> GetAllRegistrationScheduleByTeacherId(Guid teacherId);
        Task<BaseResponse<RegistrationScheduleAllResponseModel>> GetRegistrationScheduleById(int id);
        Task<BaseResponse<RegistrationScheduleAllResponseModel>> AcceptRegistrtionSchedule(AcceptRejectRegistrationScheduleRequestModel model);
        Task<BaseResponse<RegistrationScheduleAllResponseModel>> RejectRegistrtionSchedule(AcceptRejectRegistrationScheduleRequestModel model);
        Task<BaseResponse<RegistrationScheduleAllResponseModel>> CheckDupplicateRegistrtionSchedule(CheckDupplicateRegitrationScheduleRequestModel model);
        Task<BaseResponse<List<SlotResponseModel>>> GetSlotEmptyByDate(DateTime date);
    }
}
