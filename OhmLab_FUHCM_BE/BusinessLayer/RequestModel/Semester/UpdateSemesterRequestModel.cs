using System;

namespace BusinessLayer.RequestModel.Semester
{
    public class UpdateSemesterRequestModel
    {
        public string SemesterName { get; set; }
        public DateTime SemesterStartDate { get; set; }
        public DateTime SemesterEndDate { get; set; }
        public string? SemesterDescription { get; set; }
        public string SemesterStatus { get; set; }
    }
} 