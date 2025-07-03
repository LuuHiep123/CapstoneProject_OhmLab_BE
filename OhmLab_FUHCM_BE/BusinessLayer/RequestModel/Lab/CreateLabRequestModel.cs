using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Lab
{
    public class CreateLabRequestModel
    {
        public int SubjectId { get; set; }
        [Required]
        public string LabName { get; set; } = null!;
        [Required]
        public string LabRequest { get; set; } = null!;
        [Required]
        public string LabTarget { get; set; } = null!;
        [Required]
        public string LabStatus { get; set; } = null!;
    }
} 