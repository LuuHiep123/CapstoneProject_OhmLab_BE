namespace BusinessLayer.ResponseModel.ScheduleType
{
    public class ScheduleTypeResponseModel
    {
        public int ScheduleTypeId { get; set; }
        public int SlotId { get; set; }
        public string ScheduleTypeName { get; set; } = null!;
        public string? ScheduleTypeDescription { get; set; }
        public string ScheduleTypeDow { get; set; } = null!;
        public string ScheduleTypeStatus { get; set; } = null!;
        
        // Thông tin Slot
        public string? SlotName { get; set; }
        public string? SlotStartTime { get; set; }
        public string? SlotEndTime { get; set; }
        public string? SlotDescription { get; set; }
        
        // Thống kê
        public int ClassCount { get; set; }
        public int ScheduleCount { get; set; }
    }
} 