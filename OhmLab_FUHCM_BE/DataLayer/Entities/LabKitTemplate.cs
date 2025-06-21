using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class LabKitTemplate
    {
        public int LabKitTemplateId { get; set; }
        public string KitTemplateId { get; set; } = null!;
        public string LabKitTemplateStatus { get; set; } = null!;

        public virtual KitTemplate KitTemplate { get; set; } = null!;
    }
}
