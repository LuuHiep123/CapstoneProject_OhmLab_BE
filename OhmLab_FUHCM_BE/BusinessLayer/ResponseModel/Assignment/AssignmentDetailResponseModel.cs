using System;
using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.Assignment
{
    public class AssignmentDetailResponseModel
    {
        // Thông tin lịch thực hành
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; } = null!;
        public DateTime ScheduleDate { get; set; }
        public string? ScheduleDescription { get; set; }
        public string ScheduleStatus { get; set; } = null!;
        
        // Thông tin lớp học
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public string? ClassDescription { get; set; }
        public string ClassStatus { get; set; } = null!;
        
        // Thông tin môn học
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public string? SubjectDescription { get; set; }
        
        // Thông tin lab
        public int LabId { get; set; }
        public string LabName { get; set; } = null!;
        public string LabRequest { get; set; } = null!;
        public string LabTarget { get; set; } = null!;
        public string LabStatus { get; set; } = null!;
        
        // Thông tin giảng viên
        public Guid? LecturerId { get; set; }
        public string? LecturerName { get; set; }
        public string? LecturerEmail { get; set; }
        
        // Danh sách báo cáo
        public List<ReportDetailModel> Reports { get; set; } = new List<ReportDetailModel>();
        
        // Danh sách điểm
        public List<GradeDetailModel> Grades { get; set; } = new List<GradeDetailModel>();
        
        // Thống kê
        public AssignmentStatisticsModel Statistics { get; set; } = new AssignmentStatisticsModel();
        
        // Thông tin team (nếu có)
        public List<TeamDetailModel> Teams { get; set; } = new List<TeamDetailModel>();
    }
    
    public class ReportDetailModel
    {
        public int ReportId { get; set; }
        public Guid UserId { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentRollNumber { get; set; } = null!;
        public string StudentEmail { get; set; } = null!;
        public string ReportTitle { get; set; } = null!;
        public string? ReportDescription { get; set; }
        public DateTime ReportCreateDate { get; set; }
        public string ReportStatus { get; set; } = null!;
        public bool HasGrade { get; set; }
        public int? GradeId { get; set; }
        public string? GradeDescription { get; set; }
        public string? GradeStatus { get; set; }
        public string? Feedback { get; set; }
        public int DaysSinceSubmission { get; set; }
        public bool IsOverdue { get; set; }
    }
    
    public class GradeDetailModel
    {
        public int GradeId { get; set; }
        public Guid UserId { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentRollNumber { get; set; } = null!;
        public string StudentEmail { get; set; } = null!;
        public int TeamId { get; set; }
        public string? TeamName { get; set; }
        public string? GradeDescription { get; set; }
        public string GradeStatus { get; set; } = null!;
        public string? Feedback { get; set; }
        public DateTime? GradedDate { get; set; }
        public Guid? GradedByUserId { get; set; }
        public string? GradedByUserName { get; set; }
        public int DaysSinceGrading { get; set; }
        public bool IsLateGrading { get; set; }
    }
    
    public class TeamDetailModel
    {
        public int TeamId { get; set; }
        public string? TeamName { get; set; }
        public string? TeamDescription { get; set; }
        public string TeamStatus { get; set; } = null!;
        public List<TeamMemberModel> Members { get; set; } = new List<TeamMemberModel>();
        public int TotalReports { get; set; }
        public int GradedReports { get; set; }
        public double AverageGrade { get; set; }
    }
    
    public class TeamMemberModel
    {
        public Guid UserId { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentRollNumber { get; set; } = null!;
        public string StudentEmail { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int ReportsCount { get; set; }
        public double AverageGrade { get; set; }
    }
    
    public class AssignmentStatisticsModel
    {
        public int TotalStudents { get; set; }
        public int TotalReports { get; set; }
        public int SubmittedReports { get; set; }
        public int PendingReports { get; set; }
        public int OverdueReports { get; set; }
        public int TotalGrades { get; set; }
        public int GradedReports { get; set; }
        public int UngradedReports { get; set; }
        public double SubmissionRate { get; set; }
        public double GradingRate { get; set; }
        public double CompletionRate { get; set; }
        public double AverageGrade { get; set; }
        public int TotalTeams { get; set; }
        public int ActiveTeams { get; set; }
    }
} 