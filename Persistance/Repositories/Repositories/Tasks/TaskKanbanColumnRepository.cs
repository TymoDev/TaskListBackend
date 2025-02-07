using Core.DTO.TaskDTO;
using Core.Entities;
using Core.Entities.Core.Entities;
using Core.ResultModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Repositories.Tasks
{
    public class TaskKanbanColumnRepository : ITaskKanbanColumnRepository
    {
        private readonly DataContext context;

        public TaskKanbanColumnRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<KanbanColumnDto>?> GetUserColumns(Guid userId)
        {
            var user = await context.KanbanColumns.FirstOrDefaultAsync(ui => ui.UserId == userId);
            if (user == null)
            {
                return null;
            }

            var kanbanColumns = await context.KanbanColumns
             .Select(tp => new KanbanColumnDto(tp.Id, tp.Name, tp.Position))
             .ToListAsync();
            return kanbanColumns;
        }


        public async Task<KanbanColumnDto?> CreateColumn(Guid id, Guid userId, string name, int position)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }
            var data = new KanbanColumnEntity
            {
                Id = id,
                Name = name,
                Position = position,
                UserId = userId
            };

            await context.KanbanColumns.AddAsync(data);
            await context.SaveChangesAsync();

            return new KanbanColumnDto(id, name, position);
        }

        public async Task<ResultModel?> DeleteColumn(Guid taskId)
        {
            var task = await context.KanbanColumns.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null) return null;

            context.KanbanColumns.Remove(task);
            await context.SaveChangesAsync();

            return ResultModel.Ok();
        }
    }
}
