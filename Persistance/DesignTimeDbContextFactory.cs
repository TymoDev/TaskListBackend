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
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../Api/appsettings.json").Build();
            IConfigurationRoot configurationDevelopment = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../Api/appsettings.Development.json").Build();
            var builder = new DbContextOptionsBuilder<DataContext>();
            var connectionString = configurationDevelopment.GetConnectionString("DbConnection");
            builder.UseSqlite(connectionString);

            var authorizationOptions = new PersistanceAuthorizationOptions();
            configuration.GetSection("PersistanceAuthorizationOptions").Bind(authorizationOptions);

            IOptions<PersistanceAuthorizationOptions> optionsWrapper = Microsoft.Extensions.Options.Options.Create(authorizationOptions);

            return new DataContext(builder.Options, optionsWrapper);
        }
    }
}
