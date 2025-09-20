using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public partial class Lab
    {
        public Lab()
        {
            Grades = new HashSet<Grade>();
            LabEquipmentTypes = new HashSet<LabEquipmentType>();
            LabKitTemplates = new HashSet<LabKitTemplate>();
            RegistrationSchedules = new HashSet<RegistrationSchedule>();

        }

        public int LabId { get; set; }
        public int SubjectId { get; set; }
        public string LabName { get; set; } = null!;
        public string LabRequest { get; set; } = null!;
        public string LabTarget { get; set; } = null!;
        public string LabStatus { get; set; } = null!;

        public virtual Subject Subject { get; set; } = null!;
        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<LabEquipmentType> LabEquipmentTypes { get; set; }
        public virtual ICollection<LabKitTemplate> LabKitTemplates { get; set; }
        public virtual ICollection<RegistrationSchedule> RegistrationSchedules { get; set; }

    }
}
