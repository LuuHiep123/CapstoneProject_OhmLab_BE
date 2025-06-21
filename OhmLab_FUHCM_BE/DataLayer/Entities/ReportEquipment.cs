using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class ReportEquipment
    {
        public int ReportEquipmentId { get; set; }
        public string EquipmentId { get; set; } = null!;
        public string? ReportEquipmentDescription { get; set; }
        public string ReportEquipmentStatus { get; set; } = null!;

        public virtual Equipment Equipment { get; set; } = null!;
    }
}
