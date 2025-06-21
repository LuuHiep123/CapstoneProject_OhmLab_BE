using System;
using System.Collections.Generic;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataLayer.DBContext
{
    public partial class OhmLab_DBContext : DbContext
    {
        public OhmLab_DBContext()
        {
        }

        public OhmLab_DBContext(DbContextOptions<OhmLab_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accessory> Accessories { get; set; } = null!;
        public virtual DbSet<AccessoryKitTemplate> AccessoryKitTemplates { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<ClassUser> ClassUsers { get; set; } = null!;
        public virtual DbSet<Equipment> Equipment { get; set; } = null!;
        public virtual DbSet<EquipmentType> EquipmentTypes { get; set; } = null!;
        public virtual DbSet<Grade> Grades { get; set; } = null!;
        public virtual DbSet<Kit> Kits { get; set; } = null!;
        public virtual DbSet<KitAccessory> KitAccessories { get; set; } = null!;
        public virtual DbSet<KitTemplate> KitTemplates { get; set; } = null!;
        public virtual DbSet<Lab> Labs { get; set; } = null!;
        public virtual DbSet<LabEquipmentType> LabEquipmentTypes { get; set; } = null!;
        public virtual DbSet<LabKitTemplate> LabKitTemplates { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<ReportEquipment> ReportEquipments { get; set; } = null!;
        public virtual DbSet<Schedule> Schedules { get; set; } = null!;
        public virtual DbSet<ScheduleType> ScheduleTypes { get; set; } = null!;
        public virtual DbSet<Semester> Semesters { get; set; } = null!;
        public virtual DbSet<SemesterSubject> SemesterSubjects { get; set; } = null!;
        public virtual DbSet<Slot> Slots { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        public virtual DbSet<TeamEquipment> TeamEquipments { get; set; } = null!;
        public virtual DbSet<TeamKit> TeamKits { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Week> Weeks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=(local);Database=OhmLab_DB;Uid=sa;Pwd=12345;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accessory>(entity =>
            {
                entity.ToTable("Accessory");

                entity.Property(e => e.AccessoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("Accessory_id");

                entity.Property(e => e.AccessoryCase)
                    .HasMaxLength(50)
                    .HasColumnName("Accessory_Case");

                entity.Property(e => e.AccessoryCreateDate)
                    .HasColumnType("date")
                    .HasColumnName("Accessory_CreateDate");

                entity.Property(e => e.AccessoryDescription).HasColumnName("Accessory_Description");

                entity.Property(e => e.AccessoryName)
                    .HasMaxLength(50)
                    .HasColumnName("Accessory_Name");

                entity.Property(e => e.AccessoryStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Accessory_Status");

                entity.Property(e => e.AccessoryUrlImg).HasColumnName("Accessory_Url_Img");

                entity.Property(e => e.AccessoryValueCode)
                    .HasMaxLength(50)
                    .HasColumnName("Accessory_ValueCode");
            });

            modelBuilder.Entity<AccessoryKitTemplate>(entity =>
            {
                entity.ToTable("Accessory_KitTemplate");

                entity.Property(e => e.AccessoryKitTemplateId)
                    .ValueGeneratedNever()
                    .HasColumnName("Accessory_KitTemplate_id");

                entity.Property(e => e.AccessoryId).HasColumnName("Accessory_id");

                entity.Property(e => e.AccessoryKitTemplateStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Accessory_KitTemplate_Status");

                entity.Property(e => e.AccessoryQuantity).HasColumnName("Accessory_Quantity");

                entity.Property(e => e.KitTemplateId)
                    .HasMaxLength(50)
                    .HasColumnName("KitTemplate_id");

                entity.HasOne(d => d.Accessory)
                    .WithMany(p => p.AccessoryKitTemplates)
                    .HasForeignKey(d => d.AccessoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Accessory__Acces__3C69FB99");

                entity.HasOne(d => d.KitTemplate)
                    .WithMany(p => p.AccessoryKitTemplates)
                    .HasForeignKey(d => d.KitTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Accessory__KitTe__3B75D760");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class");

                entity.Property(e => e.ClassId)
                    .ValueGeneratedNever()
                    .HasColumnName("Class_id");

                entity.Property(e => e.ClassDescription).HasColumnName("Class_Description");

                entity.Property(e => e.ClassName)
                    .HasMaxLength(50)
                    .HasColumnName("Class_Name");

                entity.Property(e => e.ClassStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Class_Status");

                entity.Property(e => e.LecturerId).HasColumnName("Lecturer_id");

                entity.Property(e => e.ScheduleTypeId).HasColumnName("ScheduleType_id");

                entity.Property(e => e.SubjectId).HasColumnName("Subject_id");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.LecturerId)
                    .HasConstraintName("FK__Class__Lecturer___656C112C");

                entity.HasOne(d => d.ScheduleType)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.ScheduleTypeId)
                    .HasConstraintName("FK__Class__ScheduleT__66603565");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class__Subject_i__6477ECF3");
            });

            modelBuilder.Entity<ClassUser>(entity =>
            {
                entity.ToTable("Class_User");

                entity.Property(e => e.ClassUserId)
                    .ValueGeneratedNever()
                    .HasColumnName("Class_User_id");

                entity.Property(e => e.ClassId).HasColumnName("Class_id");

                entity.Property(e => e.ClassUserDescription).HasColumnName("Class_User_Description");

                entity.Property(e => e.ClassUserStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Class_User_Status");

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassUsers)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class_Use__Class__693CA210");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ClassUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class_Use__User___6A30C649");
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.Property(e => e.EquipmentId)
                    .HasMaxLength(50)
                    .HasColumnName("Equipment_id");

                entity.Property(e => e.EquipmentCode)
                    .HasMaxLength(50)
                    .HasColumnName("Equipment_Code");

                entity.Property(e => e.EquipmentDescription).HasColumnName("Equipment_Description");

                entity.Property(e => e.EquipmentName)
                    .HasMaxLength(50)
                    .HasColumnName("Equipment_Name");

                entity.Property(e => e.EquipmentNumberSerial)
                    .HasMaxLength(50)
                    .HasColumnName("Equipment_NumberSerial");

                entity.Property(e => e.EquipmentQr).HasColumnName("Equipment_QR");

                entity.Property(e => e.EquipmentStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Equipment_Status");

                entity.Property(e => e.EquipmentTypeId)
                    .HasMaxLength(50)
                    .HasColumnName("EquipmentType_id");

                entity.Property(e => e.EquipmentTypeUrlImg).HasColumnName("EquipmentType_Url_Img");

                entity.HasOne(d => d.EquipmentType)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.EquipmentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Equipment__Equip__5165187F");
            });

            modelBuilder.Entity<EquipmentType>(entity =>
            {
                entity.ToTable("EquipmentType");

                entity.Property(e => e.EquipmentTypeId)
                    .HasMaxLength(50)
                    .HasColumnName("EquipmentType_id");

                entity.Property(e => e.EquipmentTypeCode)
                    .HasMaxLength(50)
                    .HasColumnName("EquipmentType_Code");

                entity.Property(e => e.EquipmentTypeCreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("EquipmentType_CreateDate");

                entity.Property(e => e.EquipmentTypeDescription).HasColumnName("EquipmentType_Description");

                entity.Property(e => e.EquipmentTypeName)
                    .HasMaxLength(100)
                    .HasColumnName("EquipmentType_Name");

                entity.Property(e => e.EquipmentTypeQuantity).HasColumnName("EquipmentType_Quantity");

                entity.Property(e => e.EquipmentTypeStatus)
                    .HasMaxLength(50)
                    .HasColumnName("EquipmentType_Status");

                entity.Property(e => e.EquipmentTypeUrlImg).HasColumnName("EquipmentType_Url_Img");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.ToTable("Grade");

                entity.Property(e => e.GradeId)
                    .ValueGeneratedNever()
                    .HasColumnName("Grade_id");

                entity.Property(e => e.GradeDescription).HasColumnName("Grade_Description");

                entity.Property(e => e.GradeStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Grade_Status");

                entity.Property(e => e.LabId).HasColumnName("Lab_id");

                entity.Property(e => e.TeamId).HasColumnName("Team_id");

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.HasOne(d => d.Lab)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.LabId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Grade__Lab_id__03F0984C");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Grade__Team_id__02FC7413");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Grade__Grade_Sta__02084FDA");
            });

            modelBuilder.Entity<Kit>(entity =>
            {
                entity.ToTable("Kit");

                entity.Property(e => e.KitId)
                    .HasMaxLength(50)
                    .HasColumnName("Kit_id");

                entity.Property(e => e.KitCreateDate)
                    .HasColumnType("date")
                    .HasColumnName("Kit_CreateDate");

                entity.Property(e => e.KitDescription).HasColumnName("Kit_Description");

                entity.Property(e => e.KitName)
                    .HasMaxLength(50)
                    .HasColumnName("Kit_Name");

                entity.Property(e => e.KitStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Kit_Status");

                entity.Property(e => e.KitTemplateId)
                    .HasMaxLength(50)
                    .HasColumnName("KitTemplate_id");

                entity.Property(e => e.KitUrlImg).HasColumnName("Kit_Url_Img");

                entity.Property(e => e.KitUrlQr).HasColumnName("Kit_Url_QR");

                entity.HasOne(d => d.KitTemplate)
                    .WithMany(p => p.Kits)
                    .HasForeignKey(d => d.KitTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Kit__KitTemplate__3F466844");
            });

            modelBuilder.Entity<KitAccessory>(entity =>
            {
                entity.ToTable("Kit_Accessory");

                entity.Property(e => e.KitAccessoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("Kit_Accessory_id");

                entity.Property(e => e.AccessoryId).HasColumnName("Accessory_id");

                entity.Property(e => e.AccessoryQuantity).HasColumnName("Accessory_Quantity");

                entity.Property(e => e.KitAccessoryStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Kit_Accessory_Status");

                entity.Property(e => e.KitId)
                    .HasMaxLength(50)
                    .HasColumnName("Kit_id");

                entity.HasOne(d => d.Accessory)
                    .WithMany(p => p.KitAccessories)
                    .HasForeignKey(d => d.AccessoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Kit_Acces__Acces__4316F928");

                entity.HasOne(d => d.Kit)
                    .WithMany(p => p.KitAccessories)
                    .HasForeignKey(d => d.KitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Kit_Acces__Kit_i__4222D4EF");
            });

            modelBuilder.Entity<KitTemplate>(entity =>
            {
                entity.ToTable("KitTemplate");

                entity.Property(e => e.KitTemplateId)
                    .HasMaxLength(50)
                    .HasColumnName("KitTemplate_id");

                entity.Property(e => e.KitTemplateDescription).HasColumnName("KitTemplate_Description");

                entity.Property(e => e.KitTemplateName)
                    .HasMaxLength(50)
                    .HasColumnName("KitTemplate_Name");

                entity.Property(e => e.KitTemplateQuantity).HasColumnName("KitTemplate_Quantity");

                entity.Property(e => e.KitTemplateStatus)
                    .HasMaxLength(50)
                    .HasColumnName("KitTemplate_Status");

                entity.Property(e => e.KitTemplateUrlImg).HasColumnName("KitTemplate_Url_Img");
            });

            modelBuilder.Entity<Lab>(entity =>
            {
                entity.ToTable("Lab");

                entity.Property(e => e.LabId)
                    .ValueGeneratedNever()
                    .HasColumnName("Lab_id");

                entity.Property(e => e.LabName)
                    .HasMaxLength(50)
                    .HasColumnName("Lab_Name");

                entity.Property(e => e.LabRequest).HasColumnName("Lab_Request");

                entity.Property(e => e.LabStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Lab_Status");

                entity.Property(e => e.LabTarget).HasColumnName("Lab_Target");

                entity.Property(e => e.SubjectId).HasColumnName("Subject_id");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Labs)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lab__Lab_Status__5441852A");
            });

            modelBuilder.Entity<LabEquipmentType>(entity =>
            {
                entity.ToTable("Lab_EquipmentType");

                entity.Property(e => e.LabEquipmentTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("Lab_EquipmentType_id");

                entity.Property(e => e.EquipmentTypeId)
                    .HasMaxLength(50)
                    .HasColumnName("EquipmentType_id");

                entity.Property(e => e.LabEquipmentTypeStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Lab_EquipmentType_Status");

                entity.HasOne(d => d.EquipmentType)
                    .WithMany(p => p.LabEquipmentTypes)
                    .HasForeignKey(d => d.EquipmentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lab_Equip__Lab_E__59FA5E80");
            });

            modelBuilder.Entity<LabKitTemplate>(entity =>
            {
                entity.ToTable("Lab_KitTemplate");

                entity.Property(e => e.LabKitTemplateId)
                    .ValueGeneratedNever()
                    .HasColumnName("Lab_KitTemplate_id");

                entity.Property(e => e.KitTemplateId)
                    .HasMaxLength(50)
                    .HasColumnName("KitTemplate_id");

                entity.Property(e => e.LabKitTemplateStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Lab_KitTemplate_Status");

                entity.HasOne(d => d.KitTemplate)
                    .WithMany(p => p.LabKitTemplates)
                    .HasForeignKey(d => d.KitTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lab_KitTe__Lab_K__571DF1D5");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");

                entity.Property(e => e.ReportId)
                    .ValueGeneratedNever()
                    .HasColumnName("Report_id");

                entity.Property(e => e.ReportCreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Report_CreateDate");

                entity.Property(e => e.ReportDescription).HasColumnName("Report_Description");

                entity.Property(e => e.ReportStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Report_Status");

                entity.Property(e => e.ReportTitle)
                    .HasMaxLength(50)
                    .HasColumnName("Report_Title");

                entity.Property(e => e.ScheduleId).HasColumnName("Schedule_id");

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.ScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Report__Schedule__7C4F7684");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Report__Report_S__7B5B524B");
            });

            modelBuilder.Entity<ReportEquipment>(entity =>
            {
                entity.ToTable("Report_Equipment");

                entity.Property(e => e.ReportEquipmentId)
                    .ValueGeneratedNever()
                    .HasColumnName("Report_Equipment_id");

                entity.Property(e => e.EquipmentId)
                    .HasMaxLength(50)
                    .HasColumnName("Equipment_id");

                entity.Property(e => e.ReportEquipmentDescription).HasColumnName("Report_Equipment_Description");

                entity.Property(e => e.ReportEquipmentStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Report_Equipment_Status");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.ReportEquipments)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Report_Eq__Repor__7F2BE32F");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.ScheduleId)
                    .ValueGeneratedNever()
                    .HasColumnName("Schedule_id");

                entity.Property(e => e.ClassId).HasColumnName("Class_id");

                entity.Property(e => e.ScheduleDate)
                    .HasColumnType("date")
                    .HasColumnName("Schedule_Date");

                entity.Property(e => e.ScheduleDescription).HasColumnName("Schedule_Description");

                entity.Property(e => e.ScheduleName)
                    .HasMaxLength(50)
                    .HasColumnName("Schedule_Name");

                entity.Property(e => e.WeeksId).HasColumnName("Weeks_id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Schedule__Class___6D0D32F4");

                entity.HasOne(d => d.Weeks)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.WeeksId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Schedule__Weeks___6E01572D");
            });

            modelBuilder.Entity<ScheduleType>(entity =>
            {
                entity.ToTable("ScheduleType");

                entity.Property(e => e.ScheduleTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("ScheduleType_id");

                entity.Property(e => e.ScheduleTypeDescription).HasColumnName("ScheduleType_Description");

                entity.Property(e => e.ScheduleTypeDow)
                    .HasMaxLength(50)
                    .HasColumnName("ScheduleType_DOW");

                entity.Property(e => e.ScheduleTypeName)
                    .HasMaxLength(50)
                    .HasColumnName("ScheduleType_Name");

                entity.Property(e => e.ScheduleTypeStatus)
                    .HasMaxLength(50)
                    .HasColumnName("ScheduleType_Status");

                entity.Property(e => e.SlotId).HasColumnName("Slot_id");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.ScheduleTypes)
                    .HasForeignKey(d => d.SlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ScheduleT__Slot___5EBF139D");
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.ToTable("Semester");

                entity.Property(e => e.SemesterId)
                    .ValueGeneratedNever()
                    .HasColumnName("Semester_id");

                entity.Property(e => e.SemesterDescription).HasColumnName("Semester_Description");

                entity.Property(e => e.SemesterEndDate)
                    .HasColumnType("date")
                    .HasColumnName("Semester_EndDate");

                entity.Property(e => e.SemesterName)
                    .HasMaxLength(50)
                    .HasColumnName("Semester_Name");

                entity.Property(e => e.SemesterStartDate)
                    .HasColumnType("date")
                    .HasColumnName("Semester_StartDate");

                entity.Property(e => e.SemesterStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Semester_Status");
            });

            modelBuilder.Entity<SemesterSubject>(entity =>
            {
                entity.ToTable("Semester_Subject");

                entity.Property(e => e.SemesterSubjectId)
                    .ValueGeneratedNever()
                    .HasColumnName("Semester_Subject_id");

                entity.Property(e => e.SemesterId).HasColumnName("Semester_id");

                entity.Property(e => e.SemesterSubject1)
                    .HasMaxLength(50)
                    .HasColumnName("Semester_Subject");

                entity.Property(e => e.SubjectId).HasColumnName("Subject_id");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.SemesterSubjects)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Semester___Semes__4CA06362");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.SemesterSubjects)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Semester___Subje__4BAC3F29");
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.ToTable("Slot");

                entity.Property(e => e.SlotId)
                    .ValueGeneratedNever()
                    .HasColumnName("Slot_id");

                entity.Property(e => e.SlotDescription).HasColumnName("Slot_Description");

                entity.Property(e => e.SlotEndTime)
                    .HasMaxLength(50)
                    .HasColumnName("Slot_EndTime");

                entity.Property(e => e.SlotName)
                    .HasMaxLength(50)
                    .HasColumnName("Slot_Name");

                entity.Property(e => e.SlotStartTime)
                    .HasMaxLength(50)
                    .HasColumnName("Slot_StartTime");

                entity.Property(e => e.SlotStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Slot_Status");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject");

                entity.Property(e => e.SubjectId)
                    .ValueGeneratedNever()
                    .HasColumnName("Subject_id");

                entity.Property(e => e.SubjectCode)
                    .HasMaxLength(50)
                    .HasColumnName("Subject_Code");

                entity.Property(e => e.SubjectDescription).HasColumnName("Subject_Description");

                entity.Property(e => e.SubjectName)
                    .HasMaxLength(100)
                    .HasColumnName("Subject_Name");

                entity.Property(e => e.SubjectStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Subject_Status");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team");

                entity.Property(e => e.TeamId)
                    .ValueGeneratedNever()
                    .HasColumnName("Team_id");

                entity.Property(e => e.ClassId).HasColumnName("Class_id");

                entity.Property(e => e.TeamDescription).HasColumnName("Team_Description");

                entity.Property(e => e.TeamName)
                    .HasMaxLength(50)
                    .HasColumnName("Team_Name");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team__Class_id__70DDC3D8");
            });

            modelBuilder.Entity<TeamEquipment>(entity =>
            {
                entity.ToTable("Team_Equipment");

                entity.Property(e => e.TeamEquipmentId)
                    .ValueGeneratedNever()
                    .HasColumnName("Team_Equipment_id");

                entity.Property(e => e.EquipmentId)
                    .HasMaxLength(50)
                    .HasColumnName("Equipment_id");

                entity.Property(e => e.TeamEquipmentDateBorrow)
                    .HasColumnType("date")
                    .HasColumnName("Team_Equipment_DateBorrow");

                entity.Property(e => e.TeamEquipmentDateGiveBack)
                    .HasColumnType("date")
                    .HasColumnName("Team_Equipment_DateGiveBack");

                entity.Property(e => e.TeamEquipmentDescription).HasColumnName("Team_Equipment_Description");

                entity.Property(e => e.TeamEquipmentName)
                    .HasMaxLength(50)
                    .HasColumnName("Team_Equipment_Name");

                entity.Property(e => e.TeamEquipmentStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Team_Equipment_Status");

                entity.Property(e => e.TeamId).HasColumnName("Team_id");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.TeamEquipments)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team_Equi__Equip__74AE54BC");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamEquipments)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team_Equi__Team___73BA3083");
            });

            modelBuilder.Entity<TeamKit>(entity =>
            {
                entity.ToTable("Team_Kit");

                entity.Property(e => e.TeamKitId)
                    .ValueGeneratedNever()
                    .HasColumnName("Team_Kit_id");

                entity.Property(e => e.KitId)
                    .HasMaxLength(50)
                    .HasColumnName("Kit_id");

                entity.Property(e => e.TeamId).HasColumnName("Team_id");

                entity.Property(e => e.TeamKitDateBorrow)
                    .HasColumnType("date")
                    .HasColumnName("Team_Kit_DateBorrow");

                entity.Property(e => e.TeamKitDateGiveBack)
                    .HasColumnType("date")
                    .HasColumnName("Team_Kit_DateGiveBack");

                entity.Property(e => e.TeamKitDescription).HasColumnName("Team_Kit_Description");

                entity.Property(e => e.TeamKitName)
                    .HasMaxLength(50)
                    .HasColumnName("Team_Kit_Name");

                entity.Property(e => e.TeamKitStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Team_Kit_Status");

                entity.HasOne(d => d.Kit)
                    .WithMany(p => p.TeamKits)
                    .HasForeignKey(d => d.KitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team_Kit__Kit_id__787EE5A0");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamKits)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team_Kit__Team_i__778AC167");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("User_id");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(100)
                    .HasColumnName("User_Email");

                entity.Property(e => e.UserFullName)
                    .HasMaxLength(50)
                    .HasColumnName("User_FullName");

                entity.Property(e => e.UserNumberCode)
                    .HasMaxLength(50)
                    .HasColumnName("User_NumberCode");

                entity.Property(e => e.UserRollNumber)
                    .HasMaxLength(50)
                    .HasColumnName("User_RollNumber");
            });

            modelBuilder.Entity<Week>(entity =>
            {
                entity.HasKey(e => e.WeeksId)
                    .HasName("PK__Weeks__73E0BF1E43F15728");

                entity.Property(e => e.WeeksId)
                    .ValueGeneratedNever()
                    .HasColumnName("Weeks_id");

                entity.Property(e => e.SemesterId).HasColumnName("Semester_id");

                entity.Property(e => e.WeeksDescription).HasColumnName("Weeks_Description");

                entity.Property(e => e.WeeksEndDate)
                    .HasColumnType("date")
                    .HasColumnName("Weeks_EndDate");

                entity.Property(e => e.WeeksName)
                    .HasMaxLength(50)
                    .HasColumnName("Weeks_Name");

                entity.Property(e => e.WeeksStartDate)
                    .HasColumnType("date")
                    .HasColumnName("Weeks_StartDate");

                entity.Property(e => e.WeeksStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Weeks_Status");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Weeks)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Weeks__Semester___619B8048");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
