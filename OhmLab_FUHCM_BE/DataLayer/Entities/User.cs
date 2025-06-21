using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class User
    {
        public User()
        {
            ClassUsers = new HashSet<ClassUser>();
            Classes = new HashSet<Class>();
            Grades = new HashSet<Grade>();
            Reports = new HashSet<Report>();
        }

        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = null!;
        public string UserRollNumber { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserNumberCode { get; set; } = null!;
        public string Status { get; set; } = null!;

        public virtual ICollection<ClassUser> ClassUsers { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
