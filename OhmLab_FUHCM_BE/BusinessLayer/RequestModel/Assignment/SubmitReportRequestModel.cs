using System;

namespace BusinessLayer.RequestModel.Assignment
{
    public class SubmitReportRequestModel
    {
        public Guid UserId { get; set; }
        public int ScheduleId { get; set; }
        public string ReportTitle { get; set; }
        public string? ReportDescription { get; set; }
    }
} 