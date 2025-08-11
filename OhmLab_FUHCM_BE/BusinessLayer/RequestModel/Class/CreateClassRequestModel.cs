using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Class
{
    public class CreateClassRequestModel
    {
        public int SubjectId { get; set; }

        public Guid? LecturerId { get; set; }

        public int? ScheduleTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string ClassName { get; set; }

        [StringLength(500)]
        public string? ClassDescription { get; set; }
    }
} 