namespace BusinessLayer.ResponseModel.Lab
{
    public class LabResponseModel
    {
        public int LabId { get; set; }
        public int SubjectId { get; set; }
        public string LabName { get; set; } = null!;
        public string LabRequest { get; set; } = null!;
        public string LabTarget { get; set; } = null!;
        public string LabStatus { get; set; } = null!;
        
        // Subject details
        public string? SubjectName { get; set; }
        
        // Equipment and Kit lists
        public List<LabEquipmentResponseModel>? RequiredEquipments { get; set; }
        public List<LabKitResponseModel>? RequiredKits { get; set; }
    }
} 