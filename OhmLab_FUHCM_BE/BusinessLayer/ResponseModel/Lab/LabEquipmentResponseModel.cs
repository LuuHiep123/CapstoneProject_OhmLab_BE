namespace BusinessLayer.ResponseModel.Lab
{
    public class LabEquipmentResponseModel
    {
        public int LabEquipmentTypeId { get; set; }
        public int LabId { get; set; }
        public string EquipmentTypeId { get; set; } = null!;
        public string Status { get; set; } = null!;
        
        // Equipment Type details
        public string EquipmentTypeName { get; set; } = null!;
        public string EquipmentTypeCode { get; set; } = null!;
        public string? EquipmentTypeDescription { get; set; }
        public int EquipmentTypeQuantity { get; set; }
        public string? EquipmentTypeUrlImg { get; set; }
    }
}

