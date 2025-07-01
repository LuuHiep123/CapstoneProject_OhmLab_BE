using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Subject
{
    public class UpdateSubjectRequestModel
    {
        [Required]
        public string SubjectName { get; set; } = null!;
        public string? SubjectDescription { get; set; }
        [Required]
        public string SubjectStatus { get; set; } = null!;
    }
} 