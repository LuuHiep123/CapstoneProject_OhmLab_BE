using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class EquipmentType
    {
        public EquipmentType()
        {
            Equipment = new HashSet<Equipment>();
            LabEquipmentTypes = new HashSet<LabEquipmentType>();
        }

        public string EquipmentTypeId { get; set; } = null!;
        public string EquipmentTypeName { get; set; } = null!;
        public string EquipmentTypeCode { get; set; } = null!;
        public string? EquipmentTypeDescription { get; set; }
        public int EquipmentTypeQuantity { get; set; }
        public string? EquipmentTypeUrlImg { get; set; }
        public DateTime EquipmentTypeCreateDate { get; set; }
        public string EquipmentTypeStatus { get; set; } = null!;

        public virtual ICollection<Equipment> Equipment { get; set; }
        public virtual ICollection<LabEquipmentType> LabEquipmentTypes { get; set; }
    }
}
