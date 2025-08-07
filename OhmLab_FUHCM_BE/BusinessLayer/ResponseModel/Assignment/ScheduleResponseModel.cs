using System;

namespace BusinessLayer.ResponseModel.Assignment
{
    public class ScheduleResponseModel
    {
        public int ScheduleId { get; set; }
        public int ClassId { get; set; }
        public string ScheduleName { get; set; } = null!;
        public DateTime ScheduleDate { get; set; }
        public string? ScheduleDescription { get; set; }
        public string ClassName { get; set; }
        
        // Thông tin thêm
        public string? SubjectName { get; set; }
        public string? LecturerName { get; set; }
        public string? SlotName { get; set; }
        public string? SlotStartTime { get; set; }
        public string? SlotEndTime { get; set; }
        public string? ScheduleTypeName { get; set; }
        public string? ScheduleTypeDow { get; set; }
    }
} 