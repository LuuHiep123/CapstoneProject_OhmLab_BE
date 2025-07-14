using System;

namespace BusinessLayer.ResponseModel.Week
{
    public class WeekResponseModel
    {
        public int WeeksId { get; set; }
        public int SemesterId { get; set; }
        public string WeeksName { get; set; }
        public DateTime WeeksStartDate { get; set; }
        public DateTime WeeksEndDate { get; set; }
        public string? WeeksDescription { get; set; }
        public string WeeksStatus { get; set; }
    }
} 