using System;

namespace BusinessLayer.ResponseModel.Assignment
{
    public class ScheduleResponseModel
    {
        public int ScheduleId { get; set; }
        public int ClassId { get; set; }
        public int WeeksId { get; set; }
        public string ScheduleName { get; set; } = null!;
        public DateTime ScheduleDate { get; set; }
        public string? ScheduleDescription { get; set; }
        public string ClassName { get; set; }
        public string WeeksName { get; set; }

    }
} 