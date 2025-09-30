using System;

namespace BusinessLayer.ResponseModel.Report
{
    public class ReportResponseModel
    {
        public int ReportId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int ScheduleId { get; set; } // Now refers to RegistrationScheduleId
        public int? RegistrationScheduleId { get; set; }
        public string ScheduleName { get; set; } = null!;
        public string ReportTitle { get; set; } = null!;
        public string? ReportDescription { get; set; }
        public DateTime ReportCreateDate { get; set; }
        public string ReportStatus { get; set; } = null!;
        public string? ResolutionNotes { get; set; }
    }

    public class ReportDetailResponseModel : ReportResponseModel
    {
        public string ClassName { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public string LecturerName { get; set; } = null!;
        public DateTime ScheduleDate { get; set; }
        public string SlotName { get; set; } = null!;
        public string SlotStartTime { get; set; } = null!;
        public string SlotEndTime { get; set; } = null!;
    }

    public class ScheduleResponseModel
    {
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; } = null!;
        public DateTime ScheduleDate { get; set; }
        public string? ScheduleDescription { get; set; }
        public string ClassName { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public string LecturerName { get; set; } = null!;
        public string SlotName { get; set; } = null!;
        public string SlotStartTime { get; set; } = null!;
        public string SlotEndTime { get; set; } = null!;
    }

    public class AvailableDateResponseModel
    {
        public DateTime Date { get; set; }
        public string DayOfWeek { get; set; } = null!;
        public int ScheduleCount { get; set; }
    }

    public class SlotResponseModel
    {
        public string SlotName { get; set; } = null!;
        public string SlotStartTime { get; set; } = null!;
        public string SlotEndTime { get; set; } = null!;
        public int ScheduleCount { get; set; }
    }

    public class ClassResponseModel
    {
        public string ClassName { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public string LecturerName { get; set; } = null!;
        public int ScheduleId { get; set; }
    }

    public class ReportStatisticsResponseModel
    {
        public int TotalReports { get; set; }
        public int PendingReports { get; set; }
        public int InProgressReports { get; set; }
        public int ResolvedReports { get; set; }
        public int ClosedReports { get; set; }
        public List<ReportByStatusModel> ReportsByStatus { get; set; } = new List<ReportByStatusModel>();
        public List<ReportByMonthModel> ReportsByMonth { get; set; } = new List<ReportByMonthModel>();
    }

    public class ReportByStatusModel
    {
        public string Status { get; set; } = null!;
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class ReportByMonthModel
    {
        public string Month { get; set; } = null!;
        public int Count { get; set; }
    }
} 