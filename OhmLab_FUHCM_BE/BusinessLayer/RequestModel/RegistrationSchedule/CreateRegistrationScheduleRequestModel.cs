using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.RegistrationSchedule
{
    public class CreateRegistrationScheduleRequestModel
    {
        public string RegistrationScheduleName { get; set; } = null!;
        public Guid TeacherId { get; set; }
        public int ClassId { get; set; }
        public int LabId { get; set; }
        public int SlotId { get; set; }
        public DateTime RegistrationScheduleDate { get; set; }
        public string? RegistrationScheduleDescription { get; set; }
    }
}
