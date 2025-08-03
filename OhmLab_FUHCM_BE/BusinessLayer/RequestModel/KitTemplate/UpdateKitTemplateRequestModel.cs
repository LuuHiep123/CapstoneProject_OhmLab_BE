using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.KitTemplate
{
    public class UpdateKitTemplateRequestModel
    {
        public string KitTemplateName { get; set; } = null!;
        public int KitTemplateQuantity { get; set; }
        public string? KitTemplateDescription { get; set; }
        public string? KitTemplateUrlImg { get; set; }
        public string KitTemplateStatus { get; set; } = null!;
    }
}
