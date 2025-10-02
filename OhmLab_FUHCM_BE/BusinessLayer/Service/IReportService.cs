using BusinessLayer.RequestModel.Report;
using BusinessLayer.ResponseModel.Report;
using BusinessLayer.ResponseModel.BaseResponse;

namespace BusinessLayer.Service
{
    public interface IReportService
    {
        // CRUD Operations
        Task<BaseResponse<ReportResponseModel>> CreateReportAsync(CreateReportRequestModel model, Guid userId);
        Task<BaseResponse<ReportResponseModel>> GetReportByIdAsync(int reportId);
        Task<BaseResponse<ReportDetailResponseModel>> GetReportDetailAsync(int reportId);
        Task<BaseResponse<object>> GetAllReportsAsync();
        
        // Status Management (Admin/HeadOfDepartment only)
        Task<BaseResponse<ReportResponseModel>> UpdateReportStatusAsync(int reportId, UpdateReportStatusRequestModel model);
        
        // Statistics
        Task<BaseResponse<ReportStatisticsResponseModel>> GetReportStatisticsAsync();
        
        // User-specific reports
        Task<BaseResponse<object>> GetReportsByUserAsync(Guid userId);
        Task<BaseResponse<object>> GetReportsByRegistrationScheduleAsync(int registrationScheduleId);
        
        // Form helpers for creating reports (today only)
        Task<BaseResponse<object>> GetTodaySlotsAsync(Guid userId);
        Task<BaseResponse<object>> GetTodayClassesAsync(Guid userId, string slotName);
        
        // Incident Management (using Report entity)
        Task<BaseResponse<object>> GetPendingIncidentsAsync();
        Task<BaseResponse<object>> GetResolvedIncidentsAsync();
    }
} 