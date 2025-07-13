using System;
using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.Assignment
{
    public class AssignmentStatisticsResponseModel
    {
        // Thống kê tổng quan
        public int TotalSchedules { get; set; }
        public int TotalReports { get; set; }
        public int TotalGrades { get; set; }
        public int TotalStudents { get; set; }
        public int TotalLabs { get; set; }
        
        // Thống kê theo trạng thái
        public int SubmittedReports { get; set; }
        public int PendingReports { get; set; }
        public int GradedReports { get; set; }
        public int UngradedReports { get; set; }
        public int OverdueReports { get; set; }
        
        // Thống kê theo thời gian
        public int ReportsThisWeek { get; set; }
        public int ReportsThisMonth { get; set; }
        public int GradesThisWeek { get; set; }
        public int GradesThisMonth { get; set; }
        
        // Tỷ lệ hoàn thành
        public double SubmissionRate { get; set; }
        public double GradingRate { get; set; }
        public double CompletionRate { get; set; }
        
        // Thống kê theo lab
        public List<LabStatisticsModel> LabStatistics { get; set; } = new List<LabStatisticsModel>();
        
        // Thống kê theo sinh viên
        public List<StudentStatisticsModel> TopStudents { get; set; } = new List<StudentStatisticsModel>();
        
        // Thống kê theo thời gian
        public List<TimeSeriesData> WeeklyData { get; set; } = new List<TimeSeriesData>();
        public List<TimeSeriesData> MonthlyData { get; set; } = new List<TimeSeriesData>();
    }
    
    public class LabStatisticsModel
    {
        public int LabId { get; set; }
        public string LabName { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public int TotalReports { get; set; }
        public int GradedReports { get; set; }
        public double AverageGrade { get; set; }
        public double CompletionRate { get; set; }
    }
    
    public class StudentStatisticsModel
    {
        public Guid UserId { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentRollNumber { get; set; } = null!;
        public int TotalReports { get; set; }
        public int GradedReports { get; set; }
        public double AverageGrade { get; set; }
        public double SubmissionRate { get; set; }
    }
    
    public class TimeSeriesData
    {
        public DateTime Date { get; set; }
        public int ReportsCount { get; set; }
        public int GradesCount { get; set; }
        public double AverageGrade { get; set; }
    }
} 