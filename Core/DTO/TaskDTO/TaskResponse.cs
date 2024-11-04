using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.TaskDTO
{
    public record TaskResponse
        (
          Guid id,
          string TaskName,
          string taskStatus
        );
}
