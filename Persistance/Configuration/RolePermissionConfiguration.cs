using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistance.Options;

namespace Persistance.Configuration
{
    internal class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
    {
        private readonly PersistanceAuthorizationOptions _authorizationOptions;

        public RolePermissionConfiguration(PersistanceAuthorizationOptions authorizationOptions)
        {
            _authorizationOptions = authorizationOptions;
        }

        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.HasKey(r => new { r.RoleId, r.PermissionId });

            builder.HasData(ParseRolePermissions());
        }

        private List<RolePermissionEntity> ParseRolePermissions()
        {
            return _authorizationOptions.RolePermissions
              .SelectMany(rp => rp.Permissions
              .Select(p => new RolePermissionEntity
              {
                  RoleId = (int)Enum.Parse<Role>(rp.Role),
                  PermissionId = (int)Enum.Parse<Permission>(p)
              }))
              .ToList();
        }
    }
}
