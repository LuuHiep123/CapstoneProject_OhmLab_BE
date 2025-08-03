using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.Kit
{
    public class UpdateKitRequestModel
    {
        public string KitTemplateId { get; set; } = null!;
        public string KitName { get; set; } = null!;
        public string? KitDescription { get; set; }
        public string? KitUrlImg { get; set; }
        public string KitUrlQr { get; set; } = null!;
        public DateTime KitCreateDate { get; set; }
        public string KitStatus { get; set; } = null!;
    }
}
