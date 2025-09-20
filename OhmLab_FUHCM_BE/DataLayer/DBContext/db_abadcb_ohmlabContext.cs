using System;
using System.Collections.Generic;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace DataLayer.DBContext
{
    public partial class db_abadcb_ohmlabContext : DbContext
    {
        public db_abadcb_ohmlabContext()
        {
        }

        public db_abadcb_ohmlabContext(DbContextOptions<db_abadcb_ohmlabContext> options)
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
        public virtual DbSet<RegistrationSchedule> RegistrationSchedules { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<Schedule> Schedules { get; set; } = null!;
        public virtual DbSet<ScheduleType> ScheduleTypes { get; set; } = null!;
        public virtual DbSet<Semester> Semesters { get; set; } = null!;
        public virtual DbSet<SemesterSubject> SemesterSubjects { get; set; } = null!;
        public virtual DbSet<Slot> Slots { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        public virtual DbSet<TeamEquipment> TeamEquipments { get; set; } = null!;
        public virtual DbSet<TeamKit> TeamKits { get; set; } = null!;
        public virtual DbSet<TeamUser> TeamUsers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        string GetConnectionString()
        {
            IConfiguration builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            return builder["ConnectionStrings:hosting"];
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accessory>(entity =>
            {
                entity.ToTable("Accessory");

                entity.Property(e => e.AccessoryId).HasColumnName("Accessory_id");

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

                entity.Property(e => e.AccessoryKitTemplateId).HasColumnName("Accessory_KitTemplate_id");

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
                    .HasConstraintName("FK__Accessory__Acces__540C7B00");

                entity.HasOne(d => d.KitTemplate)
                    .WithMany(p => p.AccessoryKitTemplates)
                    .HasForeignKey(d => d.KitTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Accessory__KitTe__531856C7");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class");

                entity.Property(e => e.ClassId).HasColumnName("Class_id");

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
                    .HasConstraintName("FK__Class__Lecturer___7C1A6C5A");

                entity.HasOne(d => d.ScheduleType)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.ScheduleTypeId)
                    .HasConstraintName("FK__Class__ScheduleT__7D0E9093");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class__Subject_i__7B264821");
            });

            modelBuilder.Entity<ClassUser>(entity =>
            {
                entity.ToTable("Class_User");

                entity.Property(e => e.ClassUserId).HasColumnName("Class_User_id");

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
                    .HasConstraintName("FK__Class_Use__Class__7FEAFD3E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ClassUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Class_Use__User___00DF2177");
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
                    .HasConstraintName("FK__Equipment__Equip__690797E6");
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

                entity.Property(e => e.GradeId).HasColumnName("Grade_id");

                entity.Property(e => e.Grade1).HasColumnName("Grade");

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
                     .HasConstraintName("FK__Grade__Lab_id__1A9EF37A");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Grade__Team_id__19AACF41");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Grade__Grade_Sta__18B6AB08");
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
                    .HasConstraintName("FK__Kit__KitTemplate__56E8E7AB");
            });

            modelBuilder.Entity<KitAccessory>(entity =>
            {
                entity.ToTable("Kit_Accessory");

                entity.Property(e => e.KitAccessoryId).HasColumnName("Kit_Accessory_id");

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
                    .HasConstraintName("FK__Kit_Acces__Acces__5AB9788F");

                entity.HasOne(d => d.Kit)
                    .WithMany(p => p.KitAccessories)
                    .HasForeignKey(d => d.KitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Kit_Acces__Kit_i__59C55456");
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

                entity.Property(e => e.LabId).HasColumnName("Lab_id");

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
                    .HasConstraintName("FK__Lab__Lab_Status__6BE40491");
            });

            modelBuilder.Entity<LabEquipmentType>(entity =>
            {
                entity.ToTable("Lab_EquipmentType");

                entity.Property(e => e.LabEquipmentTypeId).HasColumnName("Lab_EquipmentType_id");

                entity.Property(e => e.EquipmentTypeId)
                    .HasMaxLength(50)
                    .HasColumnName("EquipmentType_id");

                entity.Property(e => e.LabEquipmentTypeStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Lab_EquipmentType_Status");

                entity.Property(e => e.LabId).HasColumnName("Lab_id");

                entity.HasOne(d => d.EquipmentType)
                    .WithMany(p => p.LabEquipmentTypes)
                    .HasForeignKey(d => d.EquipmentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lab_Equip__Lab_E__72910220");

                entity.HasOne(d => d.Lab)
                    .WithMany(p => p.LabEquipmentTypes)
                    .HasForeignKey(d => d.LabId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lab_Equip__Lab_i__73852659");
            });

            modelBuilder.Entity<LabKitTemplate>(entity =>
            {
                entity.ToTable("Lab_KitTemplate");

                entity.Property(e => e.LabKitTemplateId).HasColumnName("Lab_KitTemplate_id");

                entity.Property(e => e.KitTemplateId)
                    .HasMaxLength(50)
                    .HasColumnName("KitTemplate_id");

                entity.Property(e => e.LabId).HasColumnName("Lab_id");

                entity.Property(e => e.LabKitTemplateStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Lab_KitTemplate_Status");

                entity.HasOne(d => d.KitTemplate)
                    .WithMany(p => p.LabKitTemplates)
                    .HasForeignKey(d => d.KitTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lab_KitTe__Lab_K__6EC0713C");

                entity.HasOne(d => d.Lab)
                    .WithMany(p => p.LabKitTemplates)
                    .HasForeignKey(d => d.LabId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lab_KitTe__Lab_i__6FB49575");
            });

            modelBuilder.Entity<RegistrationSchedule>(entity =>
            {
                entity.ToTable("RegistrationSchedule");

                entity.Property(e => e.RegistrationScheduleId).HasColumnName("RegistrationSchedule_Id");

                entity.Property(e => e.RegistrationScheduleName)
                    .HasMaxLength(100)
                    .HasColumnName("RegistrationSchedule_Name");

                entity.Property(e => e.RegistrationScheduleDate)
                    .HasColumnType("date")
                    .HasColumnName("RegistrationSchedule_Date");

                entity.Property(e => e.RegistrationScheduleDescription)
                    .HasMaxLength(200)
                    .HasColumnName("RegistrationSchedule_Description");

                entity.Property(e => e.RegistrationScheduleNote)
                    .HasMaxLength(400)
                    .HasColumnName("RegistrationSchedule_Note");

                entity.Property(e => e.RegistrationScheduleStatus)
                    .HasMaxLength(100)
                    .HasColumnName("RegistrationSchedule_Status");

                entity.Property(e => e.TeacherId).HasColumnName("Teacher_Id");

                entity.Property(e => e.ClassId).HasColumnName("Class_Id");

                entity.Property(e => e.LabId).HasColumnName("Lab_Id");
                entity.Property(e => e.SlotId).HasColumnName("Slot_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RegistrationSchedules)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK__RegistrationSchedule__User_2");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.RegistrationSchedules)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK__RegistrationSchedule__Class__1");

                entity.HasOne(d => d.Lab)
                    .WithMany(p => p.RegistrationSchedules)
                    .HasForeignKey(d => d.LabId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RegistrationSchedule__Lab__3");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.RegistrationSchedules)
                    .HasForeignKey(d => d.SlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RegistrationSchedule__Slot__4");    
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");

                entity.Property(e => e.ReportId).HasColumnName("Report_id");

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
                    .HasConstraintName("FK__Report__Schedule__15DA3E5D");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Report__Report_S__14E61A24");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.ScheduleId).HasColumnName("Schedule_id");

                entity.Property(e => e.ClassId).HasColumnName("Class_id");

                entity.Property(e => e.ScheduleDate)
                    .HasColumnType("date")
                    .HasColumnName("Schedule_Date");

                entity.Property(e => e.ScheduleDescription).HasColumnName("Schedule_Description");

                entity.Property(e => e.ScheduleName)
                    .HasMaxLength(50)
                    .HasColumnName("Schedule_Name");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Schedule__Class___03BB8E22");
            });

            modelBuilder.Entity<ScheduleType>(entity =>
            {
                entity.ToTable("ScheduleType");

                entity.Property(e => e.ScheduleTypeId).HasColumnName("ScheduleType_id");

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
                    .HasConstraintName("FK__ScheduleT__Slot___7849DB76");
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.ToTable("Semester");

                entity.Property(e => e.SemesterId).HasColumnName("Semester_id");

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

                entity.Property(e => e.SemesterSubjectId).HasColumnName("Semester_Subject_id");

                entity.Property(e => e.SemesterId).HasColumnName("Semester_id");

                entity.Property(e => e.SemesterSubject1)
                    .HasMaxLength(50)
                    .HasColumnName("Semester_Subject");

                entity.Property(e => e.SubjectId).HasColumnName("Subject_id");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.SemesterSubjects)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Semester___Semes__6442E2C9");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.SemesterSubjects)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Semester___Subje__634EBE90");
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.ToTable("Slot");

                entity.Property(e => e.SlotId).HasColumnName("Slot_id");

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

                entity.Property(e => e.SubjectId).HasColumnName("Subject_id");

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

                entity.Property(e => e.TeamId).HasColumnName("Team_id");

                entity.Property(e => e.ClassId).HasColumnName("Class_id");

                entity.Property(e => e.TeamDescription).HasColumnName("Team_Description");

                entity.Property(e => e.TeamName)
                    .HasMaxLength(50)
                    .HasColumnName("Team_Name");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team__Class_id__0697FACD");
            });

            modelBuilder.Entity<TeamEquipment>(entity =>
            {
                entity.ToTable("Team_Equipment");

                entity.Property(e => e.TeamEquipmentId).HasColumnName("Team_Equipment_id");

                entity.Property(e => e.EquipmentId)
                    .HasMaxLength(50)
                    .HasColumnName("Equipment_id");

                entity.Property(e => e.TeamEquipmentDateBorrow)
                    .HasColumnType("datetime2")
                    .HasColumnName("Team_Equipment_DateBorrow");

                entity.Property(e => e.TeamEquipmentDateGiveBack)
                    .HasColumnType("datetime2")
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
                    .HasConstraintName("FK__Team_Equi__Equip__0E391C95");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamEquipments)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team_Equi__Team___0D44F85C");
            });

            modelBuilder.Entity<TeamKit>(entity =>
            {
                entity.ToTable("Team_Kit");

                entity.Property(e => e.TeamKitId).HasColumnName("Team_Kit_id");

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
                    .HasConstraintName("FK__Team_Kit__Kit_id__1209AD79");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamKits)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team_Kit__Team_i__11158940");
            });

            modelBuilder.Entity<TeamUser>(entity =>
            {
                entity.ToTable("Team_User");

                entity.Property(e => e.TeamUserId).HasColumnName("Team_User_id");

                entity.Property(e => e.TeamId).HasColumnName("Team_id");

                entity.Property(e => e.TeamUserStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Team_User_Status");

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamUsers)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team_User__Team___09746778");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TeamUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team_User__User___0A688BB1");
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

                entity.Property(e => e.UserRoleName)
                    .HasMaxLength(50)
                    .HasColumnName("User_RoleName");

                entity.Property(e => e.UserRollNumber)
                    .HasMaxLength(50)
                    .HasColumnName("User_RollNumber");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
