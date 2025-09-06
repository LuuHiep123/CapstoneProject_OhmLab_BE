using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Schedule
    {
        public Schedule()
        {
            Reports = new HashSet<Report>();
        }

        public int ScheduleId { get; set; }
        public int ClassId { get; set; }
        public string ScheduleName { get; set; } = null!;
        public DateTime ScheduleDate { get; set; }
        public string? ScheduleDescription { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual ICollection<Report> Reports { get; set; }
    }
}
