using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Grade
    {
        public int GradeId { get; set; }
        public Guid UserId { get; set; }
        public int TeamId { get; set; }
        public int LabId { get; set; }
        public string? GradeDescription { get; set; }
        public string GradeStatus { get; set; } = null!;

        public virtual Lab Lab { get; set; } = null!;
        public virtual Team Team { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
