using Microsoft.OpenApi.Models;


namespace SWD392_FA24_SportShop.AppStarts
{
    public static class SwaggerConfig
    {
        public static void ConfigureSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                //c.OperationFilter<SnakecasingParameOperationFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Ohm Lab API",
                    Version = "v1"
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securitySchema, new string[] { "Bearer" }}
                });
            });
            services.AddEndpointsApiExplorer();
        }
    }
}
