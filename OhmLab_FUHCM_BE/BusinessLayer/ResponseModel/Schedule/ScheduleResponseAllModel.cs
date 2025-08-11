using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModel.Schedule
{
    public class ScheduleResponseAllModel
    {
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; } = null!;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public Guid LecturerId { get; set; }
        public string LecturerName { get; set; } = null!;
        public DateTime ScheduleDate { get; set; }
        public int SlotId { get; set; }
        public string SlotName { get; set; } = null!;
        public string? SlotStartTime { get; set; }
        public string? SlotEndTime { get; set; }
        public string? ScheduleDescription { get; set; }
    }
}
