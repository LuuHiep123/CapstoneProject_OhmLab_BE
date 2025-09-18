using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.TeamEquipment
{
    public class UpdateTeamEquipmentRequestModel
    {
        public int TeamId { get; set; }
        public string EquipmentId { get; set; } = null!;
        public string TeamEquipmentName { get; set; } = null!;
        public string? TeamEquipmentDescription { get; set; }
        public DateTime TeamEquipmentDateBorrow { get; set; }
        public DateTime? TeamEquipmentDateGiveBack { get; set; }
        public string TeamEquipmentStatus { get; set; } = null!;
    }
}
