using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class TeamKit
    {
        public int TeamKitId { get; set; }
        public int TeamId { get; set; }
        public string KitId { get; set; } = null!;
        public string TeamKitName { get; set; } = null!;
        public string? TeamKitDescription { get; set; }
        public DateTime TeamKitDateBorrow { get; set; }
        public DateTime? TeamKitDateGiveBack { get; set; }
        public string TeamKitStatus { get; set; } = null!;

        public virtual Kit Kit { get; set; } = null!;
        public virtual Team Team { get; set; } = null!;
    }
}
