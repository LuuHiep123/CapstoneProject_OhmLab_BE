using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Slot
    {
        public Slot()
        {
            ScheduleTypes = new HashSet<ScheduleType>();
        }

        public int SlotId { get; set; }
        public string SlotName { get; set; } = null!;
        public string SlotStartTime { get; set; } = null!;
        public string SlotEndTime { get; set; } = null!;
        public string? SlotDescription { get; set; }
        public string SlotStatus { get; set; } = null!;

        public virtual ICollection<ScheduleType> ScheduleTypes { get; set; }
    }
}
