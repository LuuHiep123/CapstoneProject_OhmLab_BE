namespace BusinessLayer.ResponseModel.Slot
{
    public class SlotResponseModel
    {
        public int SlotId { get; set; }
        public string SlotName { get; set; } = null!;
        public string SlotStartTime { get; set; } = null!;
        public string SlotEndTime { get; set; } = null!;
        public string? SlotDescription { get; set; }
        public string SlotStatus { get; set; } = null!;


    }
} 