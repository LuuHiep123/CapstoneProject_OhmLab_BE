using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.EquipmentType
{
    public class UpdateEquipmentTypeRequestModel
    {
        public string EquipmentTypeName { get; set; } = null!;
        public string EquipmentTypeCode { get; set; } = null!;
        public string? EquipmentTypeDescription { get; set; }
        public int EquipmentTypeQuantity { get; set; }
        public string? EquipmentTypeUrlImg { get; set; }
        public string EquipmentTypeStatus { get; set; } = null!;
    }
}
