using System;

namespace BusinessLayer.RequestModel.Assignment
{
    public class UpdateScheduleRequestModel
    {
        public int ClassId { get; set; }
        public string ScheduleName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string? ScheduleDescription { get; set; }
    }
} 