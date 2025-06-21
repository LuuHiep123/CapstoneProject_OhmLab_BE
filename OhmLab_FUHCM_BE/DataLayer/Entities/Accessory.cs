using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Accessory
    {
        public Accessory()
        {
            AccessoryKitTemplates = new HashSet<AccessoryKitTemplate>();
            KitAccessories = new HashSet<KitAccessory>();
        }

        public int AccessoryId { get; set; }
        public string AccessoryName { get; set; } = null!;
        public string? AccessoryDescription { get; set; }
        public string? AccessoryUrlImg { get; set; }
        public DateTime AccessoryCreateDate { get; set; }
        public string AccessoryValueCode { get; set; } = null!;
        public string AccessoryCase { get; set; } = null!;
        public string AccessoryStatus { get; set; } = null!;

        public virtual ICollection<AccessoryKitTemplate> AccessoryKitTemplates { get; set; }
        public virtual ICollection<KitAccessory> KitAccessories { get; set; }
    }
}
