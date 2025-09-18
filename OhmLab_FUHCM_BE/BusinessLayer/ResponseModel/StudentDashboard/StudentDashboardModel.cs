using System;
using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.StudentDashboard
{
    public class StudentDashboardModel
    {
        public StudentInfoModel StudentInfo { get; set; }
        public List<UpcomingScheduleModel> UpcomingSchedules { get; set; }
        public List<AssignmentStatusModel> Assignments { get; set; }
        public GradeSummaryModel GradeSummary { get; set; }
        public List<IncidentAlertModel> RecentIncidents { get; set; }
        public NotificationSummaryModel Notifications { get; set; }
    }

    public class StudentInfoModel
    {
        public Guid UserId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public int TotalEnrolledClasses { get; set; }
        public int TotalTeams { get; set; }
    }

    public class UpcomingScheduleModel
    {
        public int ScheduleId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string ScheduleName { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int SlotId { get; set; }
        public string SlotName { get; set; }
        public string SlotStartTime { get; set; }
        public string SlotEndTime { get; set; }
        public string LabName { get; set; }
        public string LabTarget { get; set; }
        public string LecturerName { get; set; }
        public string SubjectName { get; set; }
        public int DaysUntilSchedule { get; set; }
    }

    public class AssignmentStatusModel
    {
        public int GradeId { get; set; }
        public int LabId { get; set; }
        public string LabName { get; set; }
        public string GradeStatus { get; set; }
        public int? GradeScore { get; set; }
        public string? GradeDescription { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? GradedDate { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string SubjectName { get; set; }
        public bool IsOverdue { get; set; }
        public int DaysUntilDeadline { get; set; }
    }

    public class GradeSummaryModel
    {
        public double AverageScore { get; set; }
        public int TotalAssignments { get; set; }
        public int CompletedAssignments { get; set; }
        public int PendingAssignments { get; set; }
        public int OverdueAssignments { get; set; }
        public double CompletionRate { get; set; }
        public string HighestGradeSubject { get; set; }
        public string LowestGradeSubject { get; set; }
    }

    public class IncidentAlertModel
    {
        public int ReportId { get; set; }
        public string ReportTitle { get; set; }
        public string? ReportDescription { get; set; }
        public DateTime ReportCreateDate { get; set; }
        public string ReportStatus { get; set; }
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string ClassName { get; set; }
        public int DaysSinceIncident { get; set; }
    }

    public class NotificationSummaryModel
    {
        public int UnreadNotifications { get; set; }
        public int UpcomingDeadlines { get; set; }
        public int PendingActions { get; set; }
        public int NewGrades { get; set; }
    }
}
