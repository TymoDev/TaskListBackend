using Core.Entities;
using Core.Entities.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Configuration
{
    public class TaskKanbanConfiguration : IEntityTypeConfiguration<TaskKanbanEntity>
    {
        public void Configure(EntityTypeBuilder<TaskKanbanEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(t => new { t.ColumnId, t.Order })
            .IsUnique();
        }
    }
}

