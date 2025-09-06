using System;
using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.Analytics
{
    public class LabUsageReportModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalSessions { get; set; }
        public int TotalClasses { get; set; }
        public int TotalSubjects { get; set; }
        public List<SubjectUsageModel> SubjectUsage { get; set; } = new List<SubjectUsageModel>();
        public List<SlotUsageModel> SlotUsage { get; set; } = new List<SlotUsageModel>();
        public List<LecturerUsageModel> LecturerUsage { get; set; } = new List<LecturerUsageModel>();
        public List<DailyUsageModel> DailyUsage { get; set; } = new List<DailyUsageModel>();
    }

    public class SubjectUsageModel
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int SessionCount { get; set; }
        public int ClassCount { get; set; }
        public double UsagePercentage { get; set; }
        public List<string> LecturerNames { get; set; } = new List<string>();
    }

    public class SlotUsageModel
    {
        public int SlotId { get; set; }
        public string SlotName { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SessionCount { get; set; }
        public double UsagePercentage { get; set; }
        public List<string> PopularSubjects { get; set; } = new List<string>();
    }

    public class LecturerUsageModel
    {
        public Guid LecturerId { get; set; }
        public string LecturerName { get; set; } = string.Empty;
        public string LecturerEmail { get; set; } = string.Empty;
        public int SessionCount { get; set; }
        public int ClassCount { get; set; }
        public List<string> SubjectsTaught { get; set; } = new List<string>();
        public double ActivityScore { get; set; }
    }

    public class DailyUsageModel
    {
        public DateTime Date { get; set; }
        public int SessionCount { get; set; }
        public List<string> SubjectsOnDay { get; set; } = new List<string>();
        public List<string> SlotsUsed { get; set; } = new List<string>();
    }

    public class LabUsageDetailModel
    {
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; } = string.Empty;
        public DateTime ScheduleDate { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string LecturerName { get; set; } = string.Empty;
        public string SlotName { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
