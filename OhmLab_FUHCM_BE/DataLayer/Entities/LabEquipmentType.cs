using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class LabEquipmentType
    {
        public int LabEquipmentTypeId { get; set; }
        public string EquipmentTypeId { get; set; } = null!;
        public string LabEquipmentTypeStatus { get; set; } = null!;

        public virtual EquipmentType EquipmentType { get; set; } = null!;
    }
}
