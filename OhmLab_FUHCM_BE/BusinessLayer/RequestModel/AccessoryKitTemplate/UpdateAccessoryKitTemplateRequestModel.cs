using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.AccessoryKitTemplate
{
    public class UpdateAccessoryKitTemplateRequestModel
    {
        public string KitTemplateId { get; set; } = null!;
        public int AccessoryId { get; set; }
        public int AccessoryQuantity { get; set; }
        public string AccessoryKitTemplateStatus { get; set; } = null!;
    }
}
