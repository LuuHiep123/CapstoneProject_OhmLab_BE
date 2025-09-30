using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.Grade
{
    public class ClassGradesResponseModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public List<LabInfoModel> Labs { get; set; }
        public List<StudentGradeModel> Students { get; set; }
    }

    public class LabInfoModel
    {
        public int LabId { get; set; }
        public string LabName { get; set; }
    }

    public class StudentGradeModel
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public int? TeamId { get; set; }
        public string TeamName { get; set; }
        public List<StudentLabGradeDetailModel> Grades { get; set; }
    }

    public class StudentLabGradeDetailModel
    {
        public int LabId { get; set; }
        public double? Grade { get; set; }
        public string GradeStatus { get; set; }
        public string? GradeDescription { get; set; }
        public bool IsTeamGrade { get; set; }
        public bool HasIndividualGrade { get; set; }
    }
}
