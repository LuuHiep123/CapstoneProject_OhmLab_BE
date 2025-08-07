using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.Equipment
{
    public class CreateEquipmentRequestModel
    {
        public string EquipmentTypeId { get; set; } = null!;
        public string EquipmentName { get; set; } = null!;
        public string EquipmentNumberSerial { get; set; } = null!;
        public string? EquipmentDescription { get; set; }
        public string? EquipmentTypeUrlImg { get; set; }
    }
}
