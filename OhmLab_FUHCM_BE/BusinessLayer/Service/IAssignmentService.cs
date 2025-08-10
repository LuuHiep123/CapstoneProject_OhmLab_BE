using BusinessLayer.ResponseModel.Assignment;
using BusinessLayer.ResponseModel.BaseResponse;
using DataLayer.Entities;
using BusinessLayer.RequestModel.Assignment;
using BusinessLayer.RequestModel.Class;

namespace BusinessLayer.Service
{
    public interface IAssignmentService
    {
        // Schedule management
        //Task<BaseResponse<ScheduleResponseModel>> CreatePracticeScheduleAsync(Schedule schedule);
        Task<BaseResponse<ScheduleResponseModel>> UpdatePracticeScheduleAsync(int scheduleId, Schedule schedule);
        Task<BaseResponse<bool>> DeletePracticeScheduleAsync(int scheduleId);
        Task<DynamicResponse<ScheduleResponseModel>> GetPracticeSchedulesByClassAsync(int classId);
        Task<DynamicResponse<ScheduleResponseModel>> GetPracticeSchedulesByLecturerAsync(Guid lecturerId);
        Task<BaseResponse<bool>> AddScheduleForLecturerAsync(AddScheduleForLecturerRequestModel model);
        Task<BaseResponse<bool>> AddScheduleForClassAsync(AddScheduleForClassRequestModel model);

        // Report management
        Task<BaseResponse<ReportResponseModel>> SubmitPracticeReportAsync(Report report);
        Task<BaseResponse<ReportResponseModel>> GetReportByIdAsync(int reportId);
        Task<DynamicResponse<ReportResponseModel>> GetReportsByStudentAsync(Guid studentId);
        Task<DynamicResponse<ReportResponseModel>> GetReportsByScheduleAsync(int scheduleId);
        Task<DynamicResponse<ReportResponseModel>> GetReportsByLabAsync(int labId);

        // Grade management
        Task<BaseResponse<GradeResponseModel>> GradePracticeReportAsync(Grade grade);
        Task<BaseResponse<GradeResponseModel>> UpdateGradeAsync(int gradeId, Grade grade);
        Task<BaseResponse<GradeResponseModel>> GetGradeByIdAsync(int gradeId);
        Task<DynamicResponse<GradeResponseModel>> GetGradesByLabAsync(int labId);
        Task<DynamicResponse<GradeResponseModel>> GetGradesByStudentAsync(Guid studentId);
        Task<DynamicResponse<ReportResponseModel>> GetUngradedReportsAsync();
        Task<BaseResponse<GradeResponseModel>> SubmitAssignmentAsync(Grade grade);

        // Status and feedback
        Task<BaseResponse<ReportResponseModel>> UpdateReportStatusAsync(int reportId, string status);
        Task<BaseResponse<GradeResponseModel>> AddFeedbackToGradeAsync(int gradeId, string feedback);
        Task<DynamicResponse<GradeResponseModel>> GetGradesWithFeedbackAsync(int labId);

        // Statistics
        Task<BaseResponse<object>> GetLabStatisticsAsync(int labId);
        Task<BaseResponse<object>> GetStudentGradeSummaryAsync(Guid studentId);
        Task<BaseResponse<object>> GetClassPracticeSummaryAsync(int classId);

        // Utility
        Task<Schedule> GetScheduleByIdAsync(int scheduleId);
        Task<BaseResponse<object>> GetSchedulesByDateAsync(DateTime date);
        Task<BaseResponse<object>> GetSchedulesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
} 