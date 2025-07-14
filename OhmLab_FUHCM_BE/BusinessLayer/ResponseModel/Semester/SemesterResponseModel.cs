using System;

namespace BusinessLayer.ResponseModel.Semester
{
    public class SemesterResponseModel
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public DateTime SemesterStartDate { get; set; }
        public DateTime SemesterEndDate { get; set; }
        public string? SemesterDescription { get; set; }
        public string SemesterStatus { get; set; }
    }
} 