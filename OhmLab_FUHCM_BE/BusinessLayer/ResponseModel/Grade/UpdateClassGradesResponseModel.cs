using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.Grade
{
    public class UpdateClassGradesResponseModel
    {
        public string Message { get; set; }
        public int UpdatedCount { get; set; }
        public List<UpdatedGradeItemModel> UpdatedGrades { get; set; }
    }

    public class UpdatedGradeItemModel
    {
        public string StudentId { get; set; }
        public string LabId { get; set; }
        public double Grade { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
