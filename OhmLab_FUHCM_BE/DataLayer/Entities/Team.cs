using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Team
    {
        public Team()
        {
            Grades = new HashSet<Grade>();
            TeamEquipments = new HashSet<TeamEquipment>();
            TeamKits = new HashSet<TeamKit>();
            TeamUsers = new HashSet<TeamUser>();
        }

        public int TeamId { get; set; }
        public int ClassId { get; set; }
        public string TeamName { get; set; } = null!;
        public string? TeamDescription { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<TeamEquipment> TeamEquipments { get; set; }
        public virtual ICollection<TeamKit> TeamKits { get; set; }
        public virtual ICollection<TeamUser> TeamUsers { get; set; }
    }
}
