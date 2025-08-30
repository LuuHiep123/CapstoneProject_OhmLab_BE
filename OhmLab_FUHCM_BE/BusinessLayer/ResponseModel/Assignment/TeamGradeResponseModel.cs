using System;
using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.Assignment
{
    public class TeamGradeResponseModel
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public int LabId { get; set; }
        public string LabName { get; set; } = null!;
        public int TeamGrade { get; set; }         // Điểm chung
        public string? TeamComment { get; set; }   // Nhận xét chung
        public List<TeamMemberGradeModel> Members { get; set; } = new List<TeamMemberGradeModel>();
        public DateTime? GradedDate { get; set; }
        public string GradeStatus { get; set; } = null!;
    }

    public class TeamMemberGradeModel
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public int IndividualGrade { get; set; }   // Điểm cá nhân
        public string? IndividualComment { get; set; } // Nhận xét cá nhân
    }
}

