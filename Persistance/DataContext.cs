using Core.Entities;
using DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistance.Configuration;
using Persistance.Options;

namespace Persistance
{
    public class DataContext(
        DbContextOptions<DataContext> options,
        IOptions<AuthorizationOptions> authOptions) : DbContext(options)
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));
            base.OnModelCreating(modelBuilder);
        }
    }
}

