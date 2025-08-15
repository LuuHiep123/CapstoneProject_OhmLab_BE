namespace BusinessLayer.ResponseModel.Lab
{
    public class LabKitResponseModel
    {
        public int LabKitTemplateId { get; set; }
        public int LabId { get; set; }
        public string KitTemplateId { get; set; } = null!;
        public string Status { get; set; } = null!;
        
        // Kit Template details
        public string KitTemplateName { get; set; } = null!;
        public int KitTemplateQuantity { get; set; }
        public string? KitTemplateDescription { get; set; }
        public string? KitTemplateUrlImg { get; set; }
        public string KitTemplateStatus { get; set; } = null!;
    }
}


