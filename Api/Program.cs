using Api.Extensions;
using Aplication.Services;
using Aplication.Services.User;
using BusinessLogic.Services;
using Core.Interfaces.Repositories;
using DataAccess.Repositories.RepositoriesTb;
using Infrastracture.Logic;
using Infrastracture.Logic.Authentication;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistance;
using Persistance.Repositories.Repositories;

var builder = WebApplication.CreateBuilder(args);

//DI configure services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<DataContext>
    (options =>
      options.UseSqlite(builder.Configuration.GetConnectionString("DbConnection"))  
    );
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173") 
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserGetService, UserGetService>();
builder.Services.AddScoped<IUserUpdateService,UserUpdateService>();
builder.Services.AddScoped<IUserAuthService, UserAuthService>();


builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddApiAuthentication(builder.Configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy =SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigins");
app.MapControllers();

app.Run();


public partial class Program { }