using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModel.KitAccessory
{
    public class KitAccessoryResponseModel
    {
        public int KitAccessoryId { get; set; }
        public string KitId { get; set; } = null!;
        public string KitName { get; set; } = null!;
        public int AccessoryId { get; set; }
        public string AccessoryName { get; set; }
        public string AccessoryValueCode { get; set; }
        public int AccessoryQuantity { get; set; }
        public string KitAccessoryStatus { get; set; } = null!;
    }
}
