using BusinessLayer.RequestModel.ScheduleType;
using BusinessLayer.ResponseModel.Assignment;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Report;
using BusinessLayer.ResponseModel.Schedule;
using BusinessLayer.ResponseModel.ScheduleType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IScheduleService
    {
        Task<BaseResponse<List<ScheduleResponseAllModel>>> GetAllScheduleAsync();
        Task<BaseResponse<List<ScheduleResponseAllModel>>> GetAllScheduleByStudentIdAsync(Guid userId);
        Task<BaseResponse<List<ScheduleResponseAllModel>>> GetAllScheduleByLectureIdAsync(Guid lectureId);


    }
}
