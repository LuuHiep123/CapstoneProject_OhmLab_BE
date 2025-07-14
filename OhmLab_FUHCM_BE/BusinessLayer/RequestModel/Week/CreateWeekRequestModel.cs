using System;

namespace BusinessLayer.RequestModel.Week
{
    public class CreateWeekRequestModel
    {
        public int SemesterId { get; set; }
        public string WeeksName { get; set; }
        public DateTime WeeksStartDate { get; set; }
        public DateTime WeeksEndDate { get; set; }
        public string? WeeksDescription { get; set; }
        public string WeeksStatus { get; set; }
    }
} 