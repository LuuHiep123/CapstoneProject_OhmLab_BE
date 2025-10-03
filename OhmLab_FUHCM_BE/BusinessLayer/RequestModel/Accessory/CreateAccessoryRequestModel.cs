using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel.Accessory
{
    public class CreateAccessoryRequestModel
    {
        public string AccessoryName { get; set; } = null!;
        public string? AccessoryDescription { get; set; }
        public string? AccessoryUrlImg { get; set; }
        public string AccessoryValueCode { get; set; } = null!;
        public string AccessoryCase { get; set; } = null!;
    }
}
