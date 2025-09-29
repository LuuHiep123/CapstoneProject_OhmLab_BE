using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public Guid UserId { get; set; }
        public int ScheduleId { get; set; }
        public string ReportTitle { get; set; } = null!;
        public string? ReportDescription { get; set; }
        public DateTime ReportCreateDate { get; set; }
        public string ReportStatus { get; set; } = null!;

        public virtual Schedule Schedule { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
