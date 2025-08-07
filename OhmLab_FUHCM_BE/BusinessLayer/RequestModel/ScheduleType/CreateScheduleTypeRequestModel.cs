using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.ScheduleType
{
    public class CreateScheduleTypeRequestModel
    {
        [Required]
        public int SlotId { get; set; }

        [Required]
        [StringLength(100)]
        public string ScheduleTypeName { get; set; }

        [StringLength(500)]
        public string? ScheduleTypeDescription { get; set; }

        [Required]
        [StringLength(50)]
        public string ScheduleTypeDow { get; set; }

        [Required]
        [StringLength(50)]
        public string ScheduleTypeStatus { get; set; } = "Active";
    }
} 