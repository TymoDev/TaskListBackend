using Api.Extensions;
using Aplication.Services;
using Aplication.Services.Task;
using Aplication.Services.User;
using BusinessLogic.Services;
using Core.Enums;
using Core.Interfaces.Logging;
using Core.Interfaces.Providers;
using Core.Interfaces.Repositories;
using DataAccess.Repositories.RepositoriesTb;
using Infrastracture.Auth.Authentication;
using Infrastracture.Auth.Authontication;
using Infrastracture.Authentication;
using Infrastracture.Caching;
using Infrastracture.CodesGeneration;
using Infrastracture.EmailLogic;
using Infrastracture.Logging;
using Infrastracture.Logic;
using Infrastracture.Logic.CodesGeneration;
using Infrastracture.Photos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Persistance;
using Persistance.Options;
using Persistance.Repositories.Repositories;
using Persistance.Repositories.Repositories.Tasks;
using Persistance.Repositories.Repositories.Users;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;


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
var appSettingsFile = Environment.GetEnvironmentVariable("DOTNET_APPSETTINGS_FILE") ?? "appsettings.json";
builder.Configuration
    .AddJsonFile(appSettingsFile, optional: false, reloadOnChange: true);

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

void ConfigureLogging()
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var conf = new ConfigurationBuilder()
        //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile("appsettings.Docker.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env}.json", optional: true)
        .Build();

    Serilog.Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureElasticSync(conf, env))
        .Enrich.WithProperty("Environment", env)
        .ReadFrom.Configuration(conf)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSync(IConfigurationRoot conf, string env)
{
    return new ElasticsearchSinkOptions(new Uri(conf["ElasticConfigurationDocker:Uri"]))
    {
        AutoRegisterTemplate = true,
        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace('.', '-')}-{env?.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
        NumberOfShards = 2,
        NumberOfReplicas = 1
    };
}


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


builder.Services.Configure<CloudinarySettings>(options =>
{
    options.CloudName = Environment.GetEnvironmentVariable("CloudName");
    options.ApiKey = Environment.GetEnvironmentVariable("ApiKey");
    options.ApiSecret = Environment.GetEnvironmentVariable("ApiSecret");
    options.CLOUDINARY_URL = Environment.GetEnvironmentVariable("CLOUDINARY_URL");
});

builder.Services.Configure<CacheOptions>(builder.Configuration.GetSection(nameof(CacheOptions)));
builder.Services.Configure<PersistanceAuthorizationOptions>(builder.Configuration.GetSection(nameof(PersistanceAuthorizationOptions)));


builder.Services.AddScoped<ICloudinaryLogic, CloudinaryLogic>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddSingleton<IAppLogger, SerilogAppLogger>();
builder.Services.AddSingleton<ICodeGenerator, CodeGenerator>();
builder.Services.AddScoped<ICacher, Cacher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserGetService, UserGetService>();
builder.Services.AddScoped<IUserUpdateService,UserUpdateService>();
builder.Services.AddScoped<IUserAuthService, UserAuthService>();
builder.Services.AddScoped<IUserResetPasswordService,UserResetPasswordService>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<ICookieHandler, CookieHandler>();


builder.Services.AddScoped<ITaskListRepository, TaskListRepository>();
builder.Services.AddScoped<ITaskListService, TaskListService>();
builder.Services.AddScoped<ITaskKanbanRepository, TaskKanbanRepository>();
builder.Services.AddScoped<ITaskKanbanColumnRepository, TaskKanbanColumnRepository>();
builder.Services.AddScoped<ITaskKanbanColumnService, TaskKanbanColumnService>();
builder.Services.AddScoped<ITaskKanbanService,TaskKanbanService>();


builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.AddApiRedis(builder.Configuration);
builder.Services.AddCloudinary(builder.Configuration);

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
ConfigureLogging();
builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

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