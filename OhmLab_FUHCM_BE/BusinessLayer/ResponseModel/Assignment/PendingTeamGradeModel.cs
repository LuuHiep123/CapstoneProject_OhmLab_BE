using System;

namespace BusinessLayer.ResponseModel.Assignment
{
    public class PendingTeamGradeModel
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public int LabId { get; set; }
        public string LabName { get; set; } = null!;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public DateTime? ScheduledDate { get; set; }
        public int MemberCount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, In Progress, Completed
    }
}

