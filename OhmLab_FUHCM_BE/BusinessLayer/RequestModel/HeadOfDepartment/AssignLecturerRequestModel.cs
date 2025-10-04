using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.RequestModel.HeadOfDepartment
{
    public class AssignLecturerRequestModel
    {
        [Required]
        public int ClassId { get; set; }

        [Required]
        public Guid LecturerId { get; set; }

        public string Notes { get; set; }
    }

    public class UpdateLabRegulationsRequestModel
    {
        [Required]
        public int SubjectId { get; set; }

        [Required]
        [Range(1, 10)]
        public int RequiredSlots { get; set; }

        [Required]
        [Range(1, 10)]
        public int AssignmentCount { get; set; }

        [Required]
        public string AssignmentType { get; set; } // "Individual", "Team", "Both"

        public string GradingCriteria { get; set; }

        public List<string> RequiredEquipmentTypeIds { get; set; } = new List<string>();

        public List<string> RequiredKitTemplateIds { get; set; } = new List<string>();
    }

    public class ApproveScheduleRequestModel
    {
        [Required]
        public int ScheduleId { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        public string ApprovalNotes { get; set; }
    }

    public class SubmitTimetableRequestModel
    {
        [Required]
        public int SemesterId { get; set; }

        [Required]
        public List<int> ApprovedScheduleIds { get; set; } = new List<int>();

        public string SubmissionNotes { get; set; }
    }

    public class UpdateClassRequestModel
    {
        [Required]
        public string ClassName { get; set; }

        public string ClassDescription { get; set; }

        [Required]
        public int SubjectId { get; set; }

        public int? LecturerId { get; set; }

        [Range(1, 100)]
        public int MaxStudents { get; set; } = 30;

        public string ClassStatus { get; set; } = "Active";
    }

    public class CreateClassRequestModel
    {
        [Required]
        public string ClassName { get; set; }

        public string ClassDescription { get; set; }

        [Required]
        public int SubjectId { get; set; }

        public Guid? LecturerId { get; set; }

        [Range(1, 100)]
        public int MaxStudents { get; set; } = 30;

        public string ClassStatus { get; set; } = "Active";
    }
}
