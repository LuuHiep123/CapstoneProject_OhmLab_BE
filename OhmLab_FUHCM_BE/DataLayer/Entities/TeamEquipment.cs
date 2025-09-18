 using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class TeamEquipment
    {
        public int TeamEquipmentId { get; set; }
        public int TeamId { get; set; }
        public string EquipmentId { get; set; } = null!;
        public string TeamEquipmentName { get; set; } = null!;
        public string? TeamEquipmentDescription { get; set; }
        public DateTime TeamEquipmentDateBorrow { get; set; }
        public DateTime? TeamEquipmentDateGiveBack { get; set; }
        public string TeamEquipmentStatus { get; set; } = null!;

        public virtual Equipment Equipment { get; set; } = null!;
        public virtual Team Team { get; set; } = null!;
    }
}
