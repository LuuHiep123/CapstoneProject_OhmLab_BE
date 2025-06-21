using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class KitTemplate
    {
        public KitTemplate()
        {
            AccessoryKitTemplates = new HashSet<AccessoryKitTemplate>();
            Kits = new HashSet<Kit>();
            LabKitTemplates = new HashSet<LabKitTemplate>();
        }

        public string KitTemplateId { get; set; } = null!;
        public string KitTemplateName { get; set; } = null!;
        public int KitTemplateQuantity { get; set; }
        public string? KitTemplateDescription { get; set; }
        public string? KitTemplateUrlImg { get; set; }
        public string KitTemplateStatus { get; set; } = null!;

        public virtual ICollection<AccessoryKitTemplate> AccessoryKitTemplates { get; set; }
        public virtual ICollection<Kit> Kits { get; set; }
        public virtual ICollection<LabKitTemplate> LabKitTemplates { get; set; }
    }
}
