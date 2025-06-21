using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Week
    {
        public Week()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int WeeksId { get; set; }
        public int SemesterId { get; set; }
        public string WeeksName { get; set; } = null!;
        public DateTime WeeksStartDate { get; set; }
        public DateTime WeeksEndDate { get; set; }
        public string? WeeksDescription { get; set; }
        public string WeeksStatus { get; set; } = null!;

        public virtual Semester Semester { get; set; } = null!;
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
