using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.Extensions.Options;
using Persistance.Options;


namespace Persistance
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../Api/appsettings.Development.json").Build();
            var builder = new DbContextOptionsBuilder<DataContext>();
            var connectionString = configuration.GetConnectionString("DbConnection");
            builder.UseSqlite(connectionString);

            var authorizationOptions = new AuthorizationOptions();
            configuration.GetSection("AuthorizationOptions").Bind(authorizationOptions);

            IOptions<AuthorizationOptions> optionsWrapper = Microsoft.Extensions.Options.Options.Create(authorizationOptions);

            return new DataContext(builder.Options, optionsWrapper);
        }
    }
}
