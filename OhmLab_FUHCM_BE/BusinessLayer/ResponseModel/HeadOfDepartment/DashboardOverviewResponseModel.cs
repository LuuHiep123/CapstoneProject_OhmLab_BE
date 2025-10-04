using System;
using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.HeadOfDepartment
{
    public class DashboardOverviewResponseModel
    {
        public int TotalClasses { get; set; }
        public int TotalLecturers { get; set; }
        public int TotalSubjects { get; set; }
        public int TotalSchedules { get; set; }
        public int PendingSchedules { get; set; }
        public int ApprovedSchedules { get; set; }
        public List<ClassSummaryModel> RecentClasses { get; set; } = new List<ClassSummaryModel>();
        public List<ScheduleSummaryModel> PendingApprovalSchedules { get; set; } = new List<ScheduleSummaryModel>();
    }

    public class ClassSummaryModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string LecturerName { get; set; }
        public int StudentCount { get; set; }
        public string ClassStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ScheduleSummaryModel
    {
        public int ScheduleId { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string LecturerName { get; set; }
        public string LabName { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string SlotName { get; set; }
        public string ApprovalStatus { get; set; }
        public string ScheduleDescription { get; set; }
    }

    public class LecturerAssignmentResponseModel
    {
        public Guid LecturerId { get; set; }
        public string LecturerName { get; set; }
        public string LecturerEmail { get; set; }
        public List<AssignedClassModel> AssignedClasses { get; set; } = new List<AssignedClassModel>();
        public int TotalAssignedClasses { get; set; }
        public DateTime AssignmentDate { get; set; }
    }

    public class AssignedClassModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public int StudentCount { get; set; }
        public string ClassStatus { get; set; }
    }

    public class LabRegulationsResponseModel
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int RequiredSlots { get; set; }
        public int AssignmentCount { get; set; }
        public string AssignmentType { get; set; }
        public string GradingCriteria { get; set; }
        public List<EquipmentRequirementModel> RequiredEquipments { get; set; } = new List<EquipmentRequirementModel>();
        public List<KitRequirementModel> RequiredKits { get; set; } = new List<KitRequirementModel>();
        public DateTime LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class EquipmentRequirementModel
    {
        public string EquipmentTypeId { get; set; }
        public string EquipmentTypeName { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }

    public class KitRequirementModel
    {
        public string KitTemplateId { get; set; }
        public string KitTemplateName { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }

    public class ScheduleApprovalResponseModel
    {
        public int ScheduleId { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string LecturerName { get; set; }
        public string LabName { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string SlotName { get; set; }
        public string ScheduleDescription { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovalStatus { get; set; }
        public string ApprovalNotes { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string ApprovedBy { get; set; }
    }

    public class TimetableSubmissionResponseModel
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public int TotalApprovedSchedules { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string SubmittedBy { get; set; }
        public string SubmissionStatus { get; set; }
        public List<SubmittedScheduleModel> SubmittedSchedules { get; set; } = new List<SubmittedScheduleModel>();
    }

    public class SubmittedScheduleModel
    {
        public int ScheduleId { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string LecturerName { get; set; }
        public string LabName { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string SlotName { get; set; }
        public string ApprovalNotes { get; set; }
    }

    public class LabMonitoringResponseModel
    {
        public int TotalLabSessions { get; set; }
        public int CompletedSessions { get; set; }
        public int PendingSessions { get; set; }
        public int CancelledSessions { get; set; }
        public double CompletionRate { get; set; }
        public List<SubjectStatisticsModel> SubjectStatistics { get; set; } = new List<SubjectStatisticsModel>();
        public List<LecturerPerformanceModel> LecturerPerformance { get; set; } = new List<LecturerPerformanceModel>();
        public List<EquipmentUsageModel> EquipmentUsage { get; set; } = new List<EquipmentUsageModel>();
    }

    public class SubjectStatisticsModel
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int TotalSessions { get; set; }
        public int CompletedSessions { get; set; }
        public double CompletionRate { get; set; }
        public int TotalStudents { get; set; }
        public List<string> InvolvedLecturers { get; set; } = new List<string>();
    }

    public class LecturerPerformanceModel
    {
        public Guid LecturerId { get; set; }
        public string LecturerName { get; set; }
        public int TotalAssignedClasses { get; set; }
        public int TotalLabSessions { get; set; }
        public int CompletedSessions { get; set; }
        public double CompletionRate { get; set; }
        public List<string> SubjectsTaught { get; set; } = new List<string>();
    }

    public class EquipmentUsageModel
    {
        public string EquipmentTypeId { get; set; }
        public string EquipmentTypeName { get; set; }
        public int TotalUsage { get; set; }
        public int AvailableQuantity { get; set; }
        public double UtilizationRate { get; set; }
        public List<string> UsedInSubjects { get; set; } = new List<string>();
    }

    public class ClassManagementResponseModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public string SubjectName { get; set; }
        public string LecturerName { get; set; }
        public string LecturerEmail { get; set; }
        public int StudentCount { get; set; }
        public int MaxStudents { get; set; }
        public string ClassStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ScheduleModel> Schedules { get; set; } = new List<ScheduleModel>();
    }

    public class ScheduleModel
    {
        public int ScheduleId { get; set; }
        public string LabName { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string SlotName { get; set; }
        public string ScheduleDescription { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
}
