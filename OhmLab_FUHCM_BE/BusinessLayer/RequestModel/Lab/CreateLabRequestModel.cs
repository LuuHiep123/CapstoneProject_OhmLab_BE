using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

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
        
        // Danh sách equipment cần thêm vào lab
        public List<LabEquipmentItem> RequiredEquipments { get; set; } = new List<LabEquipmentItem>();
        
        // Danh sách kit cần thêm vào lab
        public List<LabKitItem> RequiredKits { get; set; } = new List<LabKitItem>();
    }
    
    public class LabEquipmentItem
    {
        public string EquipmentTypeId { get; set; } = null!;
        public string Status { get; set; } = "Active";
    }
    
    public class LabKitItem
    {
        public string KitTemplateId { get; set; } = null!;
        public string Status { get; set; } = "Active";
    }
} 