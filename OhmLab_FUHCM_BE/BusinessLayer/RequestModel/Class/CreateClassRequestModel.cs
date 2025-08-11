using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Class
{
    public class CreateClassRequestModel
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

        // Additional fields for display
        public string? ScheduleTypeDow { get; set; }
        public string? SlotStartTime { get; set; }
        public string? SlotEndTime { get; set; }
        public string? LecturerName { get; set; }
        public string? SubjectName { get; set; }
    }
} 