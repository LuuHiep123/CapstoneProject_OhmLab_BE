using BusinessLayer.ResponseModel.Analytics;
using BusinessLayer.ResponseModel.BaseResponse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public interface IAnalyticsService
    {
        Task<BaseResponse<LabUsageReportModel>> GetLabUsageReportAsync(DateTime startDate, DateTime endDate, int? subjectId = null);
        Task<BaseResponse<LabUsageReportModel>> GetLabUsageMonthlyAsync(int year, int month);
        Task<BaseResponse<List<LabUsageDetailModel>>> GetLabUsageDetailAsync(DateTime startDate, DateTime endDate, int? subjectId = null, Guid? lecturerId = null);
        Task<BaseResponse<LabUsageReportModel>> GetLabUsageBySemesterAsync(int semesterId);
        
        // API thống kê tổng quan hệ thống
        Task<BaseResponse<SystemOverviewModel>> GetSystemOverviewAsync();
    }
}
