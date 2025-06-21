using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Subject
    {
        public Subject()
        {
            Classes = new HashSet<Class>();
            Labs = new HashSet<Lab>();
            SemesterSubjects = new HashSet<SemesterSubject>();
        }

        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public string SubjectCode { get; set; } = null!;
        public string? SubjectDescription { get; set; }
        public string SubjectStatus { get; set; } = null!;

        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Lab> Labs { get; set; }
        public virtual ICollection<SemesterSubject> SemesterSubjects { get; set; }
    }
}
