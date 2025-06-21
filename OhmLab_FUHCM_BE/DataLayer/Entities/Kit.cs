using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Kit
    {
        public Kit()
        {
            KitAccessories = new HashSet<KitAccessory>();
            TeamKits = new HashSet<TeamKit>();
        }

        public string KitId { get; set; } = null!;
        public string KitTemplateId { get; set; } = null!;
        public string KitName { get; set; } = null!;
        public string? KitDescription { get; set; }
        public string? KitUrlImg { get; set; }
        public string KitUrlQr { get; set; } = null!;
        public DateTime KitCreateDate { get; set; }
        public string KitStatus { get; set; } = null!;

        public virtual KitTemplate KitTemplate { get; set; } = null!;
        public virtual ICollection<KitAccessory> KitAccessories { get; set; }
        public virtual ICollection<TeamKit> TeamKits { get; set; }
    }
}
