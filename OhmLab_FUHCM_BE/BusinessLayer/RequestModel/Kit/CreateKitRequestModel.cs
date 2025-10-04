using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.Kit
{
    public class CreateKitRequestModel
    {
        public string KitTemplateId { get; set; } = null!;
        public string KitName { get; set; } = null!;
        public string? KitDescription { get; set; }
    }
}
