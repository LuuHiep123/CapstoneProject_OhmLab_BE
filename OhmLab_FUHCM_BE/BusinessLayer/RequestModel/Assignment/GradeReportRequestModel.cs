using System;

namespace BusinessLayer.RequestModel.Assignment
{
    public class GradeReportRequestModel
    {
        public Guid UserId { get; set; }
        public int TeamId { get; set; }
        public int LabId { get; set; }
        public string? GradeDescription { get; set; }
    }
} 