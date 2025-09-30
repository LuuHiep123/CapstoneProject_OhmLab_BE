using System;

namespace BusinessLayer.RequestModel.Assignment
{
    public class GradeTeamMemberRequestModel
    {
        public float IndividualGrade { get; set; }   // Điểm cá nhân (0-10)
        public string? IndividualComment { get; set; } // Nhận xét cá nhân
    }
}
