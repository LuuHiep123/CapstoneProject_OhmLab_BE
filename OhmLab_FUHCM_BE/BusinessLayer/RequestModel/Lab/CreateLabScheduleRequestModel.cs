using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Lab
{
    public class CreateLabScheduleRequestModel
    {
        [Required]
        public int ClassId { get; set; }
        
        [Required]
        public DateTime ScheduledDate { get; set; }
        
        [Required]
        public int SlotId { get; set; }
        
        public string? ScheduleDescription { get; set; }
        
        public int MaxStudentsPerSession { get; set; } = 30;
        
        public string? LecturerNotes { get; set; }
    }
}

