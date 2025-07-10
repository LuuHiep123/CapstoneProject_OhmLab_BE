using System;

namespace BusinessLayer.RequestModel.Assignment
{
    public class CreateScheduleRequestModel
    {
        public int ClassId { get; set; }
        public int WeeksId { get; set; }
        public string ScheduleName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string? ScheduleDescription { get; set; }
    }
} 