using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class ScheduleType
    {
        public ScheduleType()
        {
            Classes = new HashSet<Class>();
        }

        public int ScheduleTypeId { get; set; }
        public int SlotId { get; set; }
        public string ScheduleTypeName { get; set; } = null!;
        public string? ScheduleTypeDescription { get; set; }
        public string ScheduleTypeDow { get; set; } = null!;
        public string ScheduleTypeStatus { get; set; } = null!;

        public virtual Slot Slot { get; set; } = null!;
        public virtual ICollection<Class> Classes { get; set; }
    }
}
