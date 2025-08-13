namespace BusinessLayer.ResponseModel.Subject
{
    public class SubjectResponseModel
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public string SubjectCode { get; set; } = null!;
        public string? SubjectDescription { get; set; }
        public string SubjectStatus { get; set; } = null!;
        public int? SemesterId { get; set; }
        public string? SemesterName { get; set; }
    }
} 