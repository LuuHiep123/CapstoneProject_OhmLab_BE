using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class SemesterSubject
    {
        public int SemesterSubjectId { get; set; }
        public int SubjectId { get; set; }
        public int SemesterId { get; set; }
        public string SemesterSubject1 { get; set; } = null!;

        public virtual Semester Semester { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
    }
}
