using System;
using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.StudentDashboard
{
    public class EnhancedScheduleModel
    {
        public int ScheduleId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string ScheduleName { get; set; }
        public string? ScheduleDescription { get; set; }
        public ScheduleDetailModel ScheduleDetails { get; set; }
        public LabBasicInfoModel LabInfo { get; set; }
        public TeamInfoModel TeamInfo { get; set; }
        public EquipmentSummaryModel EquipmentSummary { get; set; }
        public ScheduleAssignmentStatusModel AssignmentStatus { get; set; }
        public List<IncidentSummaryModel> RelatedIncidents { get; set; }
        public bool IsPastSchedule { get; set; }
        public bool IsTodaySchedule { get; set; }
        public int DaysUntilSchedule { get; set; }
    }

    public class ScheduleDetailModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string? ClassDescription { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public Guid LecturerId { get; set; }
        public string LecturerName { get; set; }
        public string LecturerEmail { get; set; }
        public int SlotId { get; set; }
        public string SlotName { get; set; }
        public string SlotStartTime { get; set; }
        public string SlotEndTime { get; set; }
    }

    public class LabBasicInfoModel
    {
        public int LabId { get; set; }
        public string LabName { get; set; }
        public string LabTarget { get; set; }
        public string LabRequest { get; set; }
        public string LabStatus { get; set; }
    }

    public class TeamInfoModel
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string? TeamDescription { get; set; }
        public List<TeamMemberModel> Members { get; set; }
        public int MemberCount { get; set; }
    }

    public class TeamMemberModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string? ClassUserDescription { get; set; }
        public string ClassUserStatus { get; set; }
    }

    public class EquipmentSummaryModel
    {
        public int TotalEquipmentTypes { get; set; }
        public int TotalKitTemplates { get; set; }
        public List<EquipmentBasicInfoModel> EquipmentList { get; set; }
        public List<KitBasicInfoModel> KitList { get; set; }
    }

    public class EquipmentBasicInfoModel
    {
        public string EquipmentTypeId { get; set; }
        public string EquipmentTypeName { get; set; }
        public string EquipmentTypeCode { get; set; }
        public int EquipmentTypeQuantity { get; set; }
        public string? EquipmentTypeUrlImg { get; set; }
    }

    public class KitBasicInfoModel
    {
        public string KitTemplateId { get; set; }
        public string KitTemplateName { get; set; }
        public int KitTemplateQuantity { get; set; }
        public string? KitTemplateUrlImg { get; set; }
    }

    public class IncidentSummaryModel
    {
        public int ReportId { get; set; }
        public string ReportTitle { get; set; }
        public string? ReportDescription { get; set; }
        public DateTime ReportCreateDate { get; set; }
        public string ReportStatus { get; set; }
        public Guid ReporterUserId { get; set; }
        public string ReporterName { get; set; }
    }

    public class ScheduleAssignmentStatusModel
    {
        public int GradeId { get; set; }
        public string GradeStatus { get; set; }
        public double? GradeScore { get; set; }
        public string? GradeDescription { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? GradedDate { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsGraded { get; set; }
        public bool IsOverdue { get; set; }
    }
}
