using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OhmLab_FUHCM_BE.AppStarts;
using SWD392_FA24_SportShop.AppStarts;
using System.Text;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.ConfigureSwaggerServices();
builder.Services.ConfigureAuthService(builder.Configuration);


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



builder.Services.ServiceContainer(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        );
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
