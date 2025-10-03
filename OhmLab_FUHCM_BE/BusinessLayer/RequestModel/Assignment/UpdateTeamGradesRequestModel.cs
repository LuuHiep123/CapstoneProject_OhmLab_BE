using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Assignment
{
    public class UpdateTeamGradesRequestModel
    {
        [Range(0.0, 10.0, ErrorMessage = "Điểm phải từ 0 đến 10")]
        public double Grade { get; set; }
        
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? Description { get; set; }
    }
}
