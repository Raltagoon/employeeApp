using FlyingCars.EmployeeExample;
using FlyingCars.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using FlyingCars.Repositories;
using FlyingCars.Services;
using FlyingCars.Api;

var builder = WebApplication.CreateBuilder(args);
RegiSterServices(builder.Services);

var app = builder.Build();
Configure(app);
new EmployeeApi().Register(app);
new DepartmentApi().Register(app);
new PositionApi().Register(app);
new HistoryApi().Register(app);
app.Run();

void RegiSterServices(IServiceCollection services)
{
    var configuration = builder.Configuration;
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddAutoMapper(typeof(AutoMapperProfile));

    services.AddDbContext<CarContext>(options =>
    {
        options.UseMySql(configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 25)));
    });

    services.AddScoped<EmployeeRepository>();
    services.AddScoped<EmployeeService>();

    services.AddScoped<DepartmentRepository>();
    services.AddScoped<DepartmentService>();

    services.AddScoped<PositionRepository>();
    services.AddScoped<PositionService>();

    services.AddScoped<HistoryRepository>();
    services.AddScoped<HistoryService>();

    services.AddScoped<EmployeeManager>();

    services.AddAuthorization();
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });
}

void Configure(WebApplication app)
{
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    PrepDB.PrepPopulation(app);
}
