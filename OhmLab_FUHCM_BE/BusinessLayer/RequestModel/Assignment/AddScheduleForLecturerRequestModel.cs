using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Assignment
{
    public class AddScheduleForLecturerRequestModel
    {
        [Required]
        public Guid LecturerId { get; set; }

        [Required]
        public int ScheduleTypeId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}

