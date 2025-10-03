using BusinessLayer.Service.Implement;
using BusinessLayer.Service;
using DataLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using DataLayer.Repository.Implement;
using DataLayer.Repository;
using AutoMapper;


namespace OhmLab_FUHCM_BE.AppStarts
{
    public static class DependencyInjectionContainers
    {
        public static void ServiceContainer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true; ;
                options.LowercaseQueryStrings = true;
            });
            //Add_DbContext
            services.AddDbContext<db_abadcb_ohmlabContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("hosting"));
            });

            //AddService
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISubjectService>(provider => new SubjectService(
                provider.GetRequiredService<ISubjectRepository>(),
                provider.GetRequiredService<IClassRepository>(),
                provider.GetRequiredService<ISemesterRepository>(),
                provider.GetRequiredService<ISemesterSubjectRepository>(),
                provider.GetRequiredService<IMapper>(),
                provider.GetRequiredService<db_abadcb_ohmlabContext>()
            ));
            services.AddScoped<ILabService>(provider => new LabService(
                provider.GetRequiredService<ILabRepository>(),
                provider.GetRequiredService<ILabEquipmentTypeRepository>(),
                provider.GetRequiredService<ILabKitTemplateRepository>(),
                provider.GetRequiredService<IEquipmentTypeRepository>(),
                provider.GetRequiredService<IKitTemplateRepository>(),
                provider.GetRequiredService<IClassRepository>(),
                provider.GetRequiredService<IScheduleRepository>(),
                provider.GetRequiredService<IScheduleTypeRepository>(),
                provider.GetRequiredService<IMapper>()
            ));
            services.AddScoped<IEquipmentService, EquipmentService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<ISemesterRepository, SemesterRepository>();
            services.AddScoped<ISemesterService, SemesterService>();          
            services.AddScoped<ITeamEquipmentService, TeamEquipmentService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<ITeamUserService>(provider => new TeamUserService(
                provider.GetRequiredService<ITeamUserRepository>(),
                provider.GetRequiredService<ITeamRepository>(),
                provider.GetRequiredService<IUserRepository>(),
                provider.GetRequiredService<IClassUserRepository>()
            ));
            services.AddScoped<IKitTemplateService, KitTemplateService>();
            services.AddScoped<IKitService, KitService>();
            services.AddScoped<IRegistrationScheduleService, RegistrationScheduleService>();
            services.AddScoped<IClassService>(provider => new ClassService(
                provider.GetRequiredService<IScheduleRepository>(),
                provider.GetRequiredService<ISemesterSubjectRepository>(),
                provider.GetRequiredService<ISemesterRepository>(),
                provider.GetRequiredService<ISubjectRepository>(),
                provider.GetRequiredService<IScheduleTypeRepository>(),
                provider.GetRequiredService<IClassRepository>(),
                provider.GetRequiredService<IClassUserRepository>(),
                provider.GetRequiredService<ILabRepository>(),
                provider.GetRequiredService<IUserRepository>(),
                provider.GetRequiredService<IReportRepository>(),
                provider.GetRequiredService<ITeamRepository>(),
                provider.GetRequiredService<db_abadcb_ohmlabContext>(),
                provider.GetRequiredService<IMapper>()
            ));
            services.AddScoped<IClassUserService, ClassUserService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IEquipmentTypeService, EquipmentTypeService>();
            services.AddScoped<ISlotService, SlotService>();
            services.AddScoped<IScheduleTypeService, ScheduleTypeService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IAccessoryKitTemplateService, AccessoryKitTemplateService>();
            services.AddScoped<IKitAccessoryService, KitAccessoryService>();
            services.AddScoped<IAnalyticsService>(provider => new AnalyticsService(
                provider.GetRequiredService<IScheduleRepository>(),
                provider.GetRequiredService<IClassRepository>(),
                provider.GetRequiredService<IUserRepository>(),
                provider.GetRequiredService<IEquipmentRepository>(),
                provider.GetRequiredService<IReportRepository>(),
                provider.GetRequiredService<ISemesterRepository>(),
                provider.GetRequiredService<ILogger<AnalyticsService>>()
            ));
            services.AddScoped<ITeamKitService, TeamKitService>();
            services.AddScoped<IAccessoryService, AccessoryService>();

            services.AddScoped<IStudentDashboardService>(provider => new StudentDashboardService(
                provider.GetRequiredService<IClassUserRepository>(),
                provider.GetRequiredService<IScheduleRepository>(),
                provider.GetRequiredService<IGradeRepository>(),
                provider.GetRequiredService<IReportRepository>(),
                provider.GetRequiredService<ILabRepository>(),
                provider.GetRequiredService<IEquipmentTypeRepository>(),
                provider.GetRequiredService<IKitTemplateRepository>(),
                provider.GetRequiredService<ITeamRepository>(),
                provider.GetRequiredService<ITeamUserRepository>(),
                provider.GetRequiredService<IUserRepository>(),
                provider.GetRequiredService<IMapper>()
            ));


            //AddRepository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<ILabRepository, LabRepository>();
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();
            services.AddScoped<IEquipmentTypeRepository, EquipmentTypeRepository>();
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IKitAccessoryRepository, KitAccessoryRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<ITeamEquipmentRepository, TeamEquipmentRepository>();
            services.AddScoped<IKitTemplateRepository, KitTemplateRepository>();
            services.AddScoped<IKitRepository, KitRepository>();
            services.AddScoped<IClassUserRepository, ClassUserRepository>();
            services.AddScoped<IScheduleTypeRepository, ScheduleTypeRepository>();
            services.AddScoped<ISemesterSubjectRepository, SemesterSubjectRepository>();
            services.AddScoped<ISlotRepository, SlotRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<ITeamUserRepository, TeamUserRepository>();
            services.AddScoped<ILabEquipmentTypeRepository, LabEquipmentTypeRepository>();
            services.AddScoped<ILabKitTemplateRepository, LabKitTemplateRepository>();
            services.AddScoped<ITeamKitRepository, TeamKitRepository>();
            services.AddScoped<IRegistrationScheduleRepository, RegistrationScheduleRepository>();
            services.AddScoped<IAccessoryRepository, AccessoryRepository>();
            services.AddScoped<IAccessoryKitTemplateRepository, AccessoryKittemplateRepository>();
        }
    }
}
