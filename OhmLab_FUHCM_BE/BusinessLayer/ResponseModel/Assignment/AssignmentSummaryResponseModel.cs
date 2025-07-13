using System;
using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.Assignment
{
    public class AssignmentSummaryResponseModel
    {
        // Thông tin tổng quan
        public int TotalAssignments { get; set; }
        public int CompletedAssignments { get; set; }
        public int PendingAssignments { get; set; }
        public double CompletionRate { get; set; }
        
        // Thông tin theo loại
        public ScheduleSummary ScheduleSummary { get; set; } = new ScheduleSummary();
        public ReportSummary ReportSummary { get; set; } = new ReportSummary();
        public GradeSummary GradeSummary { get; set; } = new GradeSummary();
        
        // Thông tin theo thời gian
        public List<WeeklySummary> WeeklySummaries { get; set; } = new List<WeeklySummary>();
        public List<MonthlySummary> MonthlySummaries { get; set; } = new List<MonthlySummary>();
        
        // Thông tin theo đối tượng
        public List<ClassSummary> ClassSummaries { get; set; } = new List<ClassSummary>();
        public List<StudentSummary> StudentSummaries { get; set; } = new List<StudentSummary>();
        public List<LabSummary> LabSummaries { get; set; } = new List<LabSummary>();
    }
    
    public class ScheduleSummary
    {
        public int TotalSchedules { get; set; }
        public int ActiveSchedules { get; set; }
        public int CompletedSchedules { get; set; }
        public int UpcomingSchedules { get; set; }
        public double AttendanceRate { get; set; }
    }
    
    public class ReportSummary
    {
        public int TotalReports { get; set; }
        public int SubmittedReports { get; set; }
        public int PendingReports { get; set; }
        public int OverdueReports { get; set; }
        public double SubmissionRate { get; set; }
        public double OnTimeRate { get; set; }
    }
    
    public class GradeSummary
    {
        public int TotalGrades { get; set; }
        public int GradedReports { get; set; }
        public int UngradedReports { get; set; }
        public double AverageGrade { get; set; }
        public double GradingRate { get; set; }
        public double TimelyGradingRate { get; set; }
    }
    
    public class WeeklySummary
    {
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public int SchedulesCount { get; set; }
        public int ReportsCount { get; set; }
        public int GradesCount { get; set; }
        public double AverageGrade { get; set; }
    }
    
    public class MonthlySummary
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int SchedulesCount { get; set; }
        public int ReportsCount { get; set; }
        public int GradesCount { get; set; }
        public double AverageGrade { get; set; }
        public double CompletionRate { get; set; }
    }
    
    public class ClassSummary
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public int TotalStudents { get; set; }
        public int TotalSchedules { get; set; }
        public int TotalReports { get; set; }
        public int TotalGrades { get; set; }
        public double AverageGrade { get; set; }
        public double CompletionRate { get; set; }
    }
    
    public class StudentSummary
    {
        public Guid UserId { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentRollNumber { get; set; } = null!;
        public int TotalReports { get; set; }
        public int GradedReports { get; set; }
        public double AverageGrade { get; set; }
        public double SubmissionRate { get; set; }
        public int OverdueReports { get; set; }
    }
    
    public class LabSummary
    {
        public int LabId { get; set; }
        public string LabName { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public int TotalReports { get; set; }
        public int GradedReports { get; set; }
        public double AverageGrade { get; set; }
        public double CompletionRate { get; set; }
        public int DifficultyLevel { get; set; }
    }
} 