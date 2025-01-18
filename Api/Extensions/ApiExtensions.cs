using Core.Enums;
using Infrastracture.Auth.Authentication;
using Infrastracture.Auth.Authontication;
using Infrastracture.EmailLogic;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace Api.Extensions
{
    public static class ApiExtensions
    {
        //Configure Authentication
        public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["tasty-cookies"];
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddAuthorization();
        }
        public static void AddApiRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddStackExchangeRedisCache(options =>
            {
                string connection = configuration.GetConnectionString("Redis");
                options.Configuration = connection;
            });
        }
        /*public static void AddSerilog(this IServiceCollection services, IConfiguration configuration, IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, loggerConfiguration) =>
                loggerConfiguration.ReadFrom.Configuration(configuration));
        }*/
    }
}
