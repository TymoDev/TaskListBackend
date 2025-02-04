using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    namespace Core.Entities
    {
        public class TaskKanbanEntity
        {
            public Guid Id { get; set; }

            public required string TaskName { get; set; }

            public required string Column { get; set; } 
            public int Order { get; set; }  

            [ForeignKey("User")]
            public Guid UserId { get; set; }
            public UserEntity User { get; set; }
        }
    }

}
