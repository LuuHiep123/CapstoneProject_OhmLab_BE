namespace BusinessLayer.ResponseModel.Slot
{
    public class SlotResponseModel
    {
        public int SlotId { get; set; }
        public string SlotName { get; set; }
        public string SlotStartTime { get; set; }
        public string SlotEndTime { get; set; }
        public string? SlotDescription { get; set; }
        public string SlotStatus { get; set; }
    }
} 