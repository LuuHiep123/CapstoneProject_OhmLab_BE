using BusinessLayer.RequestModel.AccessoryKitTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.KitTemplate
{
    public class CreateKitTemplateRequestModel
    {
        public string KitTemplateName { get; set; } = null!;
        public string? KitTemplateDescription { get; set; }
        public string? KitTemplateUrlImg { get; set; }
        public List<ListAccessoryRequestModel> ListAccessory { get; set; }

    }
}
