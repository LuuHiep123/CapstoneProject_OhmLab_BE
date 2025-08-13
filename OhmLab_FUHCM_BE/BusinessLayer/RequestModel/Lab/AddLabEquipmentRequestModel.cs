namespace BusinessLayer.RequestModel.Lab
{
    public class AddLabEquipmentRequestModel
    {
        public string EquipmentTypeId { get; set; } = null!;
        public string Status { get; set; } = "Active";
    }
}
