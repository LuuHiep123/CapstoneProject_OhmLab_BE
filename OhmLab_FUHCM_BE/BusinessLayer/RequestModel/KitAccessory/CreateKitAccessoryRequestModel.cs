using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.KitAccessory
{
    public class CreateKitAccessoryRequestModel
    {
        public string KitId { get; set; }
        public int AccessoryId { get; set; }
        public int AccessoryQuantity { get; set; }
    }
}
