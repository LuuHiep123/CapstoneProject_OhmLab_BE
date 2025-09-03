using System;

namespace BusinessLayer.RequestModel.Assignment
{
    public class GradeTeamLabRequestModel
    {
        public int ClassId { get; set; }
        public int Grade { get; set; }             // Điểm chung cho team (0-10)
        public string? GradeDescription { get; set; } // Nhận xét chung
        public string GradeStatus { get; set; }    // Graded, Late, etc.
    }
}


