using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class TeamUser
    {
        public int TeamUserId { get; set; }
        public int TeamId { get; set; }
        public Guid UserId { get; set; }
        public string TeamUserStatus { get; set; } = null!;

        public virtual Team Team { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
