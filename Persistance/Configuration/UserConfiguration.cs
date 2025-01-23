using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public partial class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Login)
                .IsRequired();

            builder.Property(x => x.PasswordHash)
               .IsRequired();
            builder.HasIndex(x => x.Login)
             .IsUnique();
            builder.HasIndex(x => x.Email)
             .IsUnique();
            builder.HasMany(u => u.Roles)
                .WithMany(u => u.Users)
                .UsingEntity<UserRoleEntity>(
                  l => l.HasOne<RoleEntity>().WithMany().HasForeignKey(r => r.RoleId),
                  r => r.HasOne<UserEntity>().WithMany().HasForeignKey(u => u.UserId)
                );
        }
    }
}
