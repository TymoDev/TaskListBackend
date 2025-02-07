using Core.Entities.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class KanbanColumnEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string Name { get; set; } = string.Empty;

        public required int Position { get; set; }

        public List<TaskKanbanEntity> Tasks { get; set; } = new List<TaskKanbanEntity>();

        [ForeignKey("User")]
        public required Guid UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
