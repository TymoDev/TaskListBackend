using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.TaskDTO
{
    public record TaskKanbanDto(Guid taskId, string taskName, string column, Guid userId);
}
