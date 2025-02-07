using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Configuration
{
    internal class TaskKanbanColumnConfiguration : IEntityTypeConfiguration<KanbanColumnEntity>
    {
        public void Configure(EntityTypeBuilder<KanbanColumnEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Position)
                .IsUnique();
        }
    }
}
