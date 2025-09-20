using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModel.RegistrationSchedule
{
    public class RegistrationScheduleAllResponseModel
    {
        public int RegistrationScheduleId { get; set; }
        public string RegistrationScheduleName { get; set; } = null!;
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string TeacherRollNumber { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int LabId { get; set; }
        public string LabName { get; set; }
        public int SlotId { get; set; }
        public string SlotName { get; set; }
        public string SlotStartTime { get; set; }
        public string SlotEndTime { get; set; }
        public DateTime RegistrationScheduleDate { get; set; }
        public string? RegistrationScheduleDescription { get; set; }
        public string? RegistrationScheduleNote { get; set; }
        public string RegistrationScheduleStatus { get; set; } = null!;
    }
}
