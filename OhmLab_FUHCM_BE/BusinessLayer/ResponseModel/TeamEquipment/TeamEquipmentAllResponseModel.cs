using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModel.TeamEquipment
{
    public class TeamEquipmentAllResponseModel
    {
        public int TeamEquipmentId { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public string EquipmentId { get; set; } = null!;
        public string EquipmentName { get; set; } = null!;
        public string EquipmentCode { get; set; } = null!;
        public string EquipmentNumberSerial { get; set; } = null!;
        public string TeamEquipmentName { get; set; } = null!;
        public string? TeamEquipmentDescription { get; set; }
        public DateTime TeamEquipmentDateBorrow { get; set; }
        public DateTime? TeamEquipmentDateGiveBack { get; set; }
        public string TeamEquipmentStatus { get; set; } = null!;
    }
}
