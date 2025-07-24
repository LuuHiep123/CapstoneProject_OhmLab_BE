using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.TeamEquipment
{
    public class CreateTeamEquipmentRequestModel
    {
        public int TeamId { get; set; }
        public string EquipmentId { get; set; } = null!;
        public string TeamEquipmentName { get; set; } = null!;
        public string? TeamEquipmentDescription { get; set; }
    }
}
