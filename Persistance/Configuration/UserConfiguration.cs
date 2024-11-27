using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserName)
                .IsRequired();

            builder.Property(x => x.PasswordHash)
               .IsRequired();
            builder.HasIndex(x => x.UserName)
             .IsUnique();
            builder.HasIndex(x => x.Email)
             .IsUnique();
        }
    }
}
