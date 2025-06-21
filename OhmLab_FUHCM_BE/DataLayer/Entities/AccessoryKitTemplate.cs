using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class AccessoryKitTemplate
    {
        public int AccessoryKitTemplateId { get; set; }
        public string KitTemplateId { get; set; } = null!;
        public int AccessoryId { get; set; }
        public int AccessoryQuantity { get; set; }
        public string AccessoryKitTemplateStatus { get; set; } = null!;

        public virtual Accessory Accessory { get; set; } = null!;
        public virtual KitTemplate KitTemplate { get; set; } = null!;
    }
}
