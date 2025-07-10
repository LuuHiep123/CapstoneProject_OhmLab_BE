using BusinessLayer.ResponseModel.BaseResponse;
using DataLayer.Entities;

namespace BusinessLayer.Service
{
    public interface IAssignmentService
    {
        // Tạo lịch thực hành (Schedule)
        Task<BaseResponse<Schedule>> CreatePracticeScheduleAsync(Schedule schedule);
        Task<BaseResponse<Schedule>> UpdatePracticeScheduleAsync(int scheduleId, Schedule schedule);
        Task<BaseResponse<bool>> DeletePracticeScheduleAsync(int scheduleId);
        Task<DynamicResponse<Schedule>> GetPracticeSchedulesByClassAsync(int classId);
        Task<DynamicResponse<Schedule>> GetPracticeSchedulesByLecturerAsync(Guid lecturerId);
        Task<Schedule> GetScheduleByIdAsync(int scheduleId);
        
        // Sinh viên nộp bài thực hành (Report)
        Task<BaseResponse<Report>> SubmitPracticeReportAsync(Report report);
        Task<BaseResponse<Report>> GetReportByIdAsync(int reportId);
        Task<DynamicResponse<Report>> GetReportsByStudentAsync(Guid studentId);
        Task<DynamicResponse<Report>> GetReportsByScheduleAsync(int scheduleId);
        Task<DynamicResponse<Report>> GetReportsByLabAsync(int labId);
        
        // Giảng viên chấm điểm (Grade)
        Task<BaseResponse<Grade>> GradePracticeReportAsync(Grade grade);
        Task<BaseResponse<Grade>> UpdateGradeAsync(int gradeId, Grade grade);
        Task<BaseResponse<Grade>> GetGradeByIdAsync(int gradeId);
        Task<DynamicResponse<Grade>> GetGradesByLabAsync(int labId);
        Task<DynamicResponse<Grade>> GetGradesByStudentAsync(Guid studentId);
        Task<DynamicResponse<Report>> GetUngradedReportsAsync();
        
        // Phản hồi bài thực hành
        Task<BaseResponse<Report>> UpdateReportStatusAsync(int reportId, string status);
        Task<BaseResponse<Grade>> AddFeedbackToGradeAsync(int gradeId, string feedback);
        Task<DynamicResponse<Grade>> GetGradesWithFeedbackAsync(int labId);
        
        // Thống kê
        Task<BaseResponse<object>> GetLabStatisticsAsync(int labId);
        Task<BaseResponse<object>> GetStudentGradeSummaryAsync(Guid studentId);
        Task<BaseResponse<object>> GetClassPracticeSummaryAsync(int classId);
    }
} 