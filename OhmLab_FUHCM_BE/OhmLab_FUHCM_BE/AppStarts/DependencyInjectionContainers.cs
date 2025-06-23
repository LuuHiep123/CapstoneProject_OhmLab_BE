using BusinessLayer.Service.Interface;
using BusinessLayer.Service;
using DataLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using DataLayer.Repository.Implement;
using DataLayer.Repository;

namespace SWD392_FA24_SportShop.AppStarts
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

            //AddRepository
            services.AddScoped<IUserRepositoty, UserRepository>();
        }
    }
}
