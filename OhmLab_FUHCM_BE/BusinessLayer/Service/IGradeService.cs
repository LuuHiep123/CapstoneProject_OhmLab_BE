using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.RequestModel.Assignment;
using BusinessLayer.ResponseModel.Assignment;
using BusinessLayer.ResponseModel.BaseResponse;

namespace BusinessLayer.Service
{
    public interface IGradeService
    {
        // Chấm điểm team
        Task<BaseResponse<bool>> GradeTeamLabAsync(GradeTeamLabRequestModel model, int labId, int teamId, Guid lecturerId);
        
        // Chấm điểm chi tiết cho từng member
        Task<BaseResponse<bool>> GradeTeamMemberAsync(GradeTeamMemberRequestModel model, int labId, int teamId, Guid studentId, Guid lecturerId);
        
        // Xem danh sách team cần chấm điểm
        Task<BaseResponse<List<PendingTeamGradeModel>>> GetPendingTeamsAsync(int labId, Guid lecturerId);
        
        // Xem điểm của team
        Task<BaseResponse<TeamGradeResponseModel>> GetTeamGradeAsync(int labId, int teamId, Guid userId);
        
        // Xem điểm cá nhân của student
        Task<BaseResponse<TeamMemberGradeModel>> GetMyIndividualGradeAsync(int labId, Guid studentId);
        
        // Xem thống kê điểm theo team
        Task<BaseResponse<object>> GetTeamGradeStatisticsAsync(int labId, Guid lecturerId);
        
        // Xem tất cả điểm của lab (cho HeadOfDepartment)
        Task<BaseResponse<List<TeamGradeResponseModel>>> GetGradeById(int labId);
        
        // Xem tất cả điểm trong hệ thống (cho HeadOfDepartment)
        Task<BaseResponse<List<TeamGradeResponseModel>>> GetAllGrade(); 
    }
}
