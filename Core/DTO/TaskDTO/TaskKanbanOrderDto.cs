using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.TaskDTO
{
    public record TaskKanbanOrderDto(Guid taskId, string taskName, Guid column, int order, Guid userId);
}
