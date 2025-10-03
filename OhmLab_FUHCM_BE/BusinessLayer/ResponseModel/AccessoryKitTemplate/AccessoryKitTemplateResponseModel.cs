using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModel.AccessoryKitTemplate
{
    public class AccessoryKitTemplateResponseModel
    {
        public int AccessoryKitTemplateId { get; set; }
        public string KitTemplateId { get; set; }
        public string KitTemplateName { get; set; }
        public int AccessoryId { get; set; }
        public string AccessoryName { get; set; }
        public string AccessoryValueCode { get; set; }
        public int AccessoryQuantity { get; set; }
        public string AccessoryKitTemplateStatus { get; set; }
    }
}
