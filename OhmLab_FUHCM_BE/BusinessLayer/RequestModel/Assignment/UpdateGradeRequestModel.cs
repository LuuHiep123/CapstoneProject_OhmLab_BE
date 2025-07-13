using System;

namespace BusinessLayer.RequestModel.Assignment
{
    public class UpdateGradeRequestModel
    {
        public Guid UserId { get; set; }
        public int TeamId { get; set; }
        public int LabId { get; set; }
        public string? GradeDescription { get; set; }
        public string GradeStatus { get; set; }
    }
} 