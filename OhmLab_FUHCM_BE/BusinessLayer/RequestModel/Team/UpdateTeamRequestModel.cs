using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.Team
{
    public class UpdateTeamRequestModel
    {
        [Required]
        [StringLength(50)]
        public string TeamName { get; set; }

        [StringLength(500)]
        public string? TeamDescription { get; set; }

        [Required]
        [StringLength(50)]
        public string TeamStatus { get; set; }
    }
} 