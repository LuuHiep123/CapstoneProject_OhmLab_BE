using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BusinessLayer.RequestModel.Lab
{
    // ✅ THÊM MỚI: Class cho Required Equipment
    public class RequiredEquipment
    {
        [Required]
        public string EquipmentTypeId { get; set; } = null!;
        
        public string? Status { get; set; } = "Active";
    }

    // ✅ THÊM MỚI: Class cho Required Kit
    public class RequiredKit
    {
        [Required]
        public string KitTemplateId { get; set; } = null!;
        
        public string? Status { get; set; } = "Active";
    }

    public class CreateLabRequestModel
    {
        public int SubjectId { get; set; }
        [Required]
        public string LabName { get; set; } = null!;
        
        // ✅ Head of Department thiết lập quy định
        [Required]
        public string LabRequest { get; set; } = null!;        // Quy định thực hành
        [Required]
        public string LabTarget { get; set; } = null!;         // Mục tiêu lab
        
        // ✅ Thêm thông tin quy định
        public int RequiredSlots { get; set; } = 1;            // Số slot cần thiết
        public string AssignmentType { get; set; } = "Individual"; // Loại bài tập
        public int AssignmentCount { get; set; } = 1;          // Số lượng bài tập
        public string GradingCriteria { get; set; } = "";      // Tiêu chí chấm điểm
        public string? LabStatus { get; set; } = "Active";     // Trạng thái lab
        
        // ✅ Danh sách thiết bị và kit cần thiết
        public List<RequiredEquipment>? RequiredEquipments { get; set; }
        public List<RequiredKit>? RequiredKits { get; set; }
    }
} 