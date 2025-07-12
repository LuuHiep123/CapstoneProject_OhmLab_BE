namespace BusinessLayer.ResponseModel.Assignment
{
    public class GradeResponseModel
    {
        public int GradeId { get; set; }
        public Guid UserId { get; set; }
        public int TeamId { get; set; }
        public int LabId { get; set; }
        public string? GradeDescription { get; set; }
        public string GradeStatus { get; set; }
    }
} 