using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Semester
    {
        public Semester()
        {
            SemesterSubjects = new HashSet<SemesterSubject>();
        }

        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = null!;
        public DateTime SemesterStartDate { get; set; }
        public DateTime SemesterEndDate { get; set; }
        public string? SemesterDescription { get; set; }
        public string SemesterStatus { get; set; } = null!;

        public virtual ICollection<SemesterSubject> SemesterSubjects { get; set; }
    }
}
