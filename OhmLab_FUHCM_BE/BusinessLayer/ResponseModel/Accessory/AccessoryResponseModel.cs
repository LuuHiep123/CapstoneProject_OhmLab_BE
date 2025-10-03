using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModel.Accessory
{
    public class AccessoryResponseModel
    {
        public int AccessoryId { get; set; }
        public string AccessoryName { get; set; } = null!;
        public string? AccessoryDescription { get; set; }
        public string? AccessoryUrlImg { get; set; }
        public DateTime AccessoryCreateDate { get; set; }
        public string AccessoryValueCode { get; set; } = null!;
        public string AccessoryCase { get; set; } = null!;
        public string AccessoryStatus { get; set; } = null!;
    }
}
