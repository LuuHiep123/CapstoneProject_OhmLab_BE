using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.Equipment
{
    public class CreateEquipmentRequestModel
    {
        public string EquipmentName { get; set; } = null!;
        public string EquipmentCode { get; set; } = null!;
        public string EquipmentNumberSerial { get; set; } = null!;
        public string? EquipmentDescription { get; set; }
        public string? EquipmentTypeUrlImg { get; set; }
        public string EquipmentQr { get; set; } = null!;
        public string EquipmentStatus { get; set; } = null!;
    }
}
