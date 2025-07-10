using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.Equipment
{
    public class UpdateEquipmentRequestModel
    {
        public string? EquipmentName { get; set; }
        public string? EquipmentCode { get; set; }
        public string? EquipmentNumberSerial { get; set; }
        public string? EquipmentDescription { get; set; }
        public string? EquipmentTypeUrlImg { get; set; }
        public string? EquipmentQr { get; set; }
        public string? EquipmentStatus { get; set; }
    }
}
