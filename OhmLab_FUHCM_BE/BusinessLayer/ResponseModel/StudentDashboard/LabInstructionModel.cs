using System.Collections.Generic;

namespace BusinessLayer.ResponseModel.StudentDashboard
{
    public class LabInstructionModel
    {
        public int LabId { get; set; }
        public string LabName { get; set; }
        public string LabTarget { get; set; }
        public string LabRequest { get; set; }
        public string LabStatus { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public List<EquipmentInstructionModel> RequiredEquipments { get; set; }
        public List<KitInstructionModel> RequiredKits { get; set; }
        public string EstimatedDuration { get; set; }
        public string DifficultyLevel { get; set; }
        public List<SafetyNoteModel> SafetyNotes { get; set; }
    }

    public class EquipmentInstructionModel
    {
        public string EquipmentTypeId { get; set; }
        public string EquipmentTypeName { get; set; }
        public string EquipmentTypeCode { get; set; }
        public string? EquipmentTypeDescription { get; set; }
        public int EquipmentTypeQuantity { get; set; }
        public string? EquipmentTypeUrlImg { get; set; }
        public string EquipmentTypeStatus { get; set; }
    }

    public class KitInstructionModel
    {
        public string KitTemplateId { get; set; }
        public string KitTemplateName { get; set; }
        public int KitTemplateQuantity { get; set; }
        public string? KitTemplateDescription { get; set; }
        public string? KitTemplateUrlImg { get; set; }
        public string KitTemplateStatus { get; set; }
    }

    public class SafetyNoteModel
    {
        public int NoteId { get; set; }
        public string NoteTitle { get; set; }
        public string NoteContent { get; set; }
        public string NoteType { get; set; }
        public int Priority { get; set; }
    }
}
