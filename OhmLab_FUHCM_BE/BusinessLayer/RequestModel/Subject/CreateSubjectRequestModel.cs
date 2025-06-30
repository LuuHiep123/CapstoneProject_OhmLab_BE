using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Subject
{
    public class CreateSubjectRequestModel
    {
        [Required]
        public string SubjectName { get; set; } = null!;
        [Required]
        public string SubjectCode { get; set; } = null!;
        public string? SubjectDescription { get; set; }
    }
} 