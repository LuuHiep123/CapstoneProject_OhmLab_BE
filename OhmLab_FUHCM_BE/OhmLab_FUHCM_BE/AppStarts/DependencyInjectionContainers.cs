﻿using BusinessLayer.Service.Implement;
using BusinessLayer.Service;
using DataLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using DataLayer.Repository.Implement;
using DataLayer.Repository;
using BusinessLayer.Service.Interface;


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
            services.AddDbContext<OhmLab_DBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("hosting"));
            });

            //AddService
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ILabService, LabService>();

            //AddRepository
            services.AddScoped<IUserRepositoty, UserRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<ILabRepository, LabRepository>();
        }
    }
}
