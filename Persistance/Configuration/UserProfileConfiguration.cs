using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Configuration
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfileEntity>
    {
        public void Configure(EntityTypeBuilder<UserProfileEntity> builder)
        {
            builder
             .HasOne(up => up.ProfileImage)
             .WithOne()
             .HasForeignKey<UserProfileEntity>(up => up.ProfileImageId)
             .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
