using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.TaskDTO
{
   public record TaskKanbanUpdateDto(Guid taskId, string taskName,int order, Guid columnId);
}
