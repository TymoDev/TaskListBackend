using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TaskEntity
    {
        public Guid Id { get; set; }
        public required string TaskName { get; set; }
        public required string TaskStatus { get; set; }

      //  [ForeignKey("User")]
      //  public Guid UserId { get; set; }
      // public UserEntity User { get; set; }
    }
}
