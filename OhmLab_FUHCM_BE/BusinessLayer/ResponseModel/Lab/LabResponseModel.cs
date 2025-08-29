using System.Text.Json;

namespace BusinessLayer.ResponseModel.Lab
{
    public class LabResponseModel
    {
        public int LabId { get; set; }
        public int SubjectId { get; set; }
        public string LabName { get; set; } = null!;
        public string LabRequest { get; set; } = null!;
        public string LabTarget { get; set; } = null!;
        public string? LabStatus { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }
        
        // ✅ THÊM MỚI: Required equipment và kits
        public List<LabEquipmentResponseModel>? RequiredEquipments { get; set; }
        public List<LabKitResponseModel>? RequiredKits { get; set; }
        
        // ✅ THÊM MỚI: Slot info (không đụng database)
        public int? SlotId { get; set; }
        public string? SlotName { get; set; }
        public string? SlotStartTime { get; set; }
        public string? SlotEndTime { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
    }
} 