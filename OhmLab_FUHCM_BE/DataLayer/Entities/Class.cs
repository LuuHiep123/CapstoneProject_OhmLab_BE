using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Class
    {
        public Class()
        {
            ClassUsers = new HashSet<ClassUser>();
            Schedules = new HashSet<Schedule>();
            Teams = new HashSet<Team>();
            RegistrationSchedules = new HashSet<RegistrationSchedule>();
        }

        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public Guid? LecturerId { get; set; }
        public int? ScheduleTypeId { get; set; }
        public string ClassName { get; set; } = null!;
        public string? ClassDescription { get; set; }
        public string ClassStatus { get; set; } = null!;

        public virtual User? Lecturer { get; set; }
        public virtual ScheduleType? ScheduleType { get; set; }
        public virtual Subject Subject { get; set; } = null!;
        public virtual ICollection<ClassUser> ClassUsers { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<RegistrationSchedule> RegistrationSchedules { get; set; }

    }
}
