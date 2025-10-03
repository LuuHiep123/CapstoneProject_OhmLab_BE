using System;

namespace BusinessLayer.ResponseModel.Assignment
{
    public class GradeResponseModel
    {
        public int GradeId { get; set; }
        public Guid UserId { get; set; }
        public int TeamId { get; set; }
        public int LabId { get; set; }
        public string? GradeDescription { get; set; }
        public string GradeStatus { get; set; }
        public double IndividualGrade { get; set; }  // Điểm cá nhân (có thể đã được điều chỉnh)
        public double TeamGrade { get; set; }        // Điểm gốc của team
        public bool IsAdjusted => IndividualGrade != TeamGrade;  // Tự động tính dựa trên sự khác biệt giữa IndividualGrade và TeamGrade
    }
}