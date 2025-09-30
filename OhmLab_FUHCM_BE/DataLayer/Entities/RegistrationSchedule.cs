using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public partial class RegistrationSchedule
    {
        public int RegistrationScheduleId { get; set; }
        public string RegistrationScheduleName { get; set; } = null!;
        public Guid TeacherId { get; set; }
        public int ClassId { get; set; }
        public int LabId { get; set; }
        public int SlotId { get; set; }
        public DateTime RegistrationScheduleDate { get; set; }
        public string? RegistrationScheduleDescription { get; set; }
        public string RegistrationScheduleStatus { get; set; } = null!;
        public string? RegistrationScheduleNote { get; set; }
        public DateTime RegistrationScheduleCreateDate { get; set; }


        public virtual User User { get; set; } = null!;
        public virtual Class Class { get; set; } = null!;
        public virtual Lab Lab { get; set; } = null!;
        public virtual Slot Slot { get; set; } = null!;
        public virtual ICollection<Report> Reports { get; set; }
    }
}
