using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Slot
{
    public class CreateSlotRequestModel
    {
        [Required]
        [StringLength(50)]
        public string SlotName { get; set; }

        [Required]
        [StringLength(50)]
        public string SlotStartTime { get; set; }

        [Required]
        [StringLength(50)]
        public string SlotEndTime { get; set; }

        [StringLength(500)]
        public string? SlotDescription { get; set; }

        [Required]
        [StringLength(50)]
        public string SlotStatus { get; set; } = "Active";
    }
} 