using System;
using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.Grade
{
    public class StudentLabGradesResponseModel
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public List<StudentLabGradeModel> LabGrades { get; set; }
    }

    public class StudentLabGradeModel
    {
        public int GradeId { get; set; }
        public int LabId { get; set; }
        public string LabName { get; set; }
        public string LabTarget { get; set; }
        public string SubjectName { get; set; }
        public string ClassName { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public double? GradeScore { get; set; }
        public string? GradeDescription { get; set; }
        public string GradeStatus { get; set; }
        public bool IsTeamGrade { get; set; }
        public bool HasIndividualGrade { get; set; }
        public string LecturerName { get; set; }
    }
}
