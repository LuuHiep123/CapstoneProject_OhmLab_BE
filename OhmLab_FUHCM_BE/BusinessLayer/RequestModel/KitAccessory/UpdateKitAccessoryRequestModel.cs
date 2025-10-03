using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.KitAccessory
{
    public class UpdateKitAccessoryRequestModel
    {
        public string KitId { get; set; } = null!;
        public int AccessoryId { get; set; }
        public int AccessoryQuantity { get; set; }
        public string KitAccessoryStatus { get; set; } = null!;
    }
}
