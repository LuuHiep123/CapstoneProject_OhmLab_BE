using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.Class
{
    public class UpdateClassRequestModel
    {
        [Required]
        public int SubjectId { get; set; }

        public Guid? LecturerId { get; set; }

        public int? ScheduleTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string ClassName { get; set; }

        [StringLength(500)]
        public string? ClassDescription { get; set; }

        [Required]
        [StringLength(50)]
        public string ClassStatus { get; set; } = "Active";

        // Additional fields for display
        public string? ScheduleTypeDow { get; set; }
        public string? SlotStartTime { get; set; }
        public string? SlotEndTime { get; set; }
        public string? LecturerName { get; set; }
        public string? SubjectName { get; set; }
    }
}
