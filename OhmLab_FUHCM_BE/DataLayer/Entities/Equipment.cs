using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Equipment
    {
        public Equipment()
        {
            ReportEquipments = new HashSet<ReportEquipment>();
            TeamEquipments = new HashSet<TeamEquipment>();
        }

        public string EquipmentId { get; set; } = null!;
        public string EquipmentTypeId { get; set; } = null!;
        public string EquipmentName { get; set; } = null!;
        public string EquipmentCode { get; set; } = null!;
        public string EquipmentNumberSerial { get; set; } = null!;
        public string? EquipmentDescription { get; set; }
        public string? EquipmentTypeUrlImg { get; set; }
        public string EquipmentQr { get; set; } = null!;
        public string EquipmentStatus { get; set; } = null!;

        public virtual EquipmentType EquipmentType { get; set; } = null!;
        public virtual ICollection<ReportEquipment> ReportEquipments { get; set; }
        public virtual ICollection<TeamEquipment> TeamEquipments { get; set; }
    }
}
