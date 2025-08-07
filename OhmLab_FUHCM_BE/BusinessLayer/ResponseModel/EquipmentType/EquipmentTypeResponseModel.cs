using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModel.EquipmentType
{
    public class EquipmentTypeResponseModel
    {
        public string EquipmentTypeId { get; set; } = null!;
        public string EquipmentTypeName { get; set; } = null!;
        public string EquipmentTypeCode { get; set; } = null!;
        public string? EquipmentTypeDescription { get; set; }
        public int EquipmentTypeQuantity { get; set; }
        public string? EquipmentTypeUrlImg { get; set; }
        public DateTime EquipmentTypeCreateDate { get; set; }
        public string EquipmentTypeStatus { get; set; } = null!;
    }
}
