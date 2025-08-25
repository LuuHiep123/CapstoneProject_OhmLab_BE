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
        
        // ✅ THÊM MỚI: Các trường quy định thực hành (parse từ JSON)
        public int TotalSlots { get; set; }                    // ✅ Số buổi cần làm của bài lab (từ JSON requiredSlots)
        public int TotalAssignments { get; set; }              // ✅ Tổng lượng bài lab (sẽ lấy từ LabService)
        public string AssignmentType { get; set; } = "";
        public int AssignmentCount { get; set; }
        public string GradingCriteria { get; set; } = "";
        
        // ✅ THÊM MỚI: Parse JSON từ LabRequest để lấy quy định
        public void ParseLabRequest()
        {
            if (!string.IsNullOrEmpty(LabRequest))
            {
                try
                {
                    var regulation = JsonSerializer.Deserialize<LabRegulation>(LabRequest);
                    if (regulation != null)
                    {
                        // ✅ Lấy RequiredSlots từ JSON để hiển thị số buổi cần làm
                        TotalSlots = regulation.RequiredSlots;
                        AssignmentType = regulation.AssignmentType ?? "";
                        AssignmentCount = regulation.AssignmentCount;
                        GradingCriteria = regulation.GradingCriteria ?? "";
                    }
                }
                catch
                {
                    // Nếu parse JSON lỗi thì giữ nguyên giá trị mặc định
                }
            }
        }
        
        // ✅ THÊM MỚI: Class để deserialize JSON
        private class LabRegulation
        {
            public int RequiredSlots { get; set; }             // ✅ Số buổi cần làm
            public string? AssignmentType { get; set; }
            public int AssignmentCount { get; set; }
            public string? GradingCriteria { get; set; }
        }
        
        // Subject details
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }
        
        // ✅ THÊM MỚI: Required equipment và kits
        public List<LabEquipmentResponseModel>? RequiredEquipments { get; set; }
        public List<LabKitResponseModel>? RequiredKits { get; set; }
    }
} 