using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class KitAccessory
    {
        public int KitAccessoryId { get; set; }
        public string KitId { get; set; } = null!;
        public int AccessoryId { get; set; }
        public int AccessoryQuantity { get; set; }
        public string KitAccessoryStatus { get; set; } = null!;

        public virtual Accessory Accessory { get; set; } = null!;
        public virtual Kit Kit { get; set; } = null!;
    }
}
