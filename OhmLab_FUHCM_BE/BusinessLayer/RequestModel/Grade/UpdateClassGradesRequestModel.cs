using System.Collections.Generic;

namespace BusinessLayer.RequestModel.Grade
{
    public class UpdateClassGradesRequestModel
    {
        public List<UpdateGradeItemModel> Grades { get; set; }
    }

    public class UpdateGradeItemModel
    {
        public string StudentId { get; set; }
        public string LabId { get; set; }
        public double Grade { get; set; }
        public string? GradeDescription { get; set; }
        public string? GradeStatus { get; set; }
    }
}
