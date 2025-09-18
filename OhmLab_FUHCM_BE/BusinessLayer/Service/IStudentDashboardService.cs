using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.StudentDashboard;

namespace BusinessLayer.Service
{
    public interface IStudentDashboardService
    {
        Task<BaseResponse<StudentDashboardModel>> GetStudentDashboardAsync(Guid studentId);
        Task<BaseResponse<LabInstructionModel>> GetLabInstructionsAsync(int labId, Guid studentId);
        Task<BaseResponse<List<EnhancedScheduleModel>>> GetEnhancedScheduleAsync(
            Guid studentId, DateTime? startDate, DateTime? endDate);
    }
}
