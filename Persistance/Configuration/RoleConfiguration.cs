using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public partial class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasMany(r => r.Permissions)
                .WithMany(r => r.Roles)
                .UsingEntity<RolePermissionEntity>(
                  l => l.HasOne<PermissionEntity>().WithMany().HasForeignKey(e => e.PermissionId),
                  r => r.HasOne<RoleEntity>().WithMany().HasForeignKey(e => e.RoleId)
                );
            var roles = Enum.
                GetValues<Role>()
                .Select(r => new RoleEntity
                {
                    Id = (int)r,
                    Name = r.ToString(),
                });
            builder.HasData(roles);
            
        }
    }
}

