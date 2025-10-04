using BusinessLayer.RequestModel.HeadOfDepartment;
using BusinessLayer.ResponseModel.HeadOfDepartment;
using BusinessLayer.ResponseModel.BaseResponse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IHeadOfDepartmentService
    {
        // Dashboard Overview - Chỉ giữ lại dashboard tổng quan
        Task<BaseResponse<DashboardOverviewResponseModel>> GetDashboardOverview();

        // Lab Monitoring - Chỉ giữ lại các API thống kê
        Task<BaseResponse<LabMonitoringResponseModel>> GetLabMonitoringOverview(MonitorLabRequestModel model);
        Task<BaseResponse<List<SubjectStatisticsModel>>> GetSubjectStatistics();
        Task<BaseResponse<List<LecturerPerformanceModel>>> GetLecturerPerformance();
        Task<BaseResponse<List<EquipmentUsageModel>>> GetEquipmentUsage();
    }
}
