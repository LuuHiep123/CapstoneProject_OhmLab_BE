using System;

namespace BusinessLayer.ResponseModel.Assignment
{
    public class ReportResponseModel
    {
        public int ReportId { get; set; }
        public Guid UserId { get; set; }
        public int ScheduleId { get; set; }
        public string ReportTitle { get; set; }
        public string? ReportDescription { get; set; }
        public DateTime ReportCreateDate { get; set; }
        public string ReportStatus { get; set; }
    }
} 