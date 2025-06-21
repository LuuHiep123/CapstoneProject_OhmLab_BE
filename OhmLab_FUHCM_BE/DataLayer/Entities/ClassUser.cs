using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class ClassUser
    {
        public int ClassUserId { get; set; }
        public int ClassId { get; set; }
        public Guid UserId { get; set; }
        public string? ClassUserDescription { get; set; }
        public string ClassUserStatus { get; set; } = null!;

        public virtual Class Class { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
