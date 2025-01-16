using Api.Extensions;
using Aplication.Services;
using Aplication.Services.User;
using BusinessLogic.Services;
using Core.Enums;
using Core.Interfaces.Logging;
using Core.Interfaces.Providers;
using Core.Interfaces.Repositories;
using DataAccess.Repositories.RepositoriesTb;
using Elastic.CommonSchema;
using Infrastracture.Auth.Authentication;
using Infrastracture.Auth.Authontication;
using Infrastracture.Authentication;
using Infrastracture.Caching;
using Infrastracture.CodesGeneration;
using Infrastracture.EmailLogic;
using Infrastracture.Logging;
using Infrastracture.Logic;
using Infrastracture.Logic.CodesGeneration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Persistance;
using Persistance.Options;
using Persistance.Repositories.Repositories;
using Serilog;

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
            builder.WithOrigins("http://localhost:5173", "https://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});


DotNetEnv.Env.Load();
builder.Configuration.AddEnvironmentVariables();


builder.Services.Configure<JwtOptions>(options =>
{
    options.SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
    options.ExpiresHours = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRES_HOURS") ?? "1");
});

builder.Services.Configure<EmailOptions>(options =>
{
    options.SmtpServer = Environment.GetEnvironmentVariable("EMAIL_SMTP_SERVER");
    options.Port = int.Parse(Environment.GetEnvironmentVariable("EMAIL_PORT") ?? "587");
    options.UseSsl = bool.Parse(Environment.GetEnvironmentVariable("EMAIL_USE_SSL") ?? "true");
    options.FromEmail = Environment.GetEnvironmentVariable("EMAIL_FROM");
    options.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
});
builder.Services.Configure<CacheOptions>(builder.Configuration.GetSection(nameof(CacheOptions)));
builder.Services.Configure<PersistanceAuthorizationOptions>(builder.Configuration.GetSection(nameof(PersistanceAuthorizationOptions)));



builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddSingleton<IAppLogger, SerilogAppLogger>();
builder.Services.AddScoped<ICacher, Cacher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserGetService, UserGetService>();
builder.Services.AddScoped<IUserUpdateService,UserUpdateService>();
builder.Services.AddScoped<IUserAuthService, UserAuthService>();
builder.Services.AddScoped<IUserResetPasswordService,UserResetPasswordService>();


builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddSingleton<ICodeGenerator, CodeGenerator>();

builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.AddApiRedis(builder.Configuration);
builder.Services.AddSerilog(builder.Configuration, builder.Host);

builder.Services.AddAuthorization(options =>
{
    foreach (var permission in Enum.GetValues<Permission>())
    {
        var policyName = $"Permissions:{permission}";
        options.AddPolicy(policyName, policy =>
        {
            policy.Requirements.Add(new PermissionRequirement(new[] { permission }));
        });
    }
});

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
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


public partial class Program { }