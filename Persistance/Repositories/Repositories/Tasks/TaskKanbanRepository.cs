using Core.DTO.TaskDTO;
using Core.Entities;
using Core.Entities.Core.Entities;
using Core.ResultModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories.Repositories.Tasks
{
    public class TaskKanbanRepository : ITaskKanbanRepository
    {
        private readonly DataContext context;

        public TaskKanbanRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<TaskKanbanOrderDto>?> GetUserTasks(Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }
            var kanbanTasks = await context.KanbanTasks
            .OrderBy(tp => tp.Order)
            .Select(tp => new TaskKanbanOrderDto(tp.Id, tp.TaskName, tp.ColumnId, tp.Order, tp.UserId))
            .ToListAsync();
            return kanbanTasks;
        }


        public async Task<TaskKanbanOrderDto?> CreateTask(string taskName, Guid columnId, Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            var column = await context.KanbanColumns.FindAsync(columnId);
            if (user == null)
            {
                return null;
            }

            var order = await GetNextOrderInColumn(columnId, userId);
            var newKanbanTask = new TaskKanbanEntity
            {
                Id = Guid.NewGuid(),
                TaskName = taskName,
                Column = column,
                Order = order,
                UserId = userId
            };

            await context.KanbanTasks.AddAsync(newKanbanTask);
            await context.SaveChangesAsync();

            return new TaskKanbanOrderDto(newKanbanTask.Id, newKanbanTask.TaskName, newKanbanTask.ColumnId, order, userId);
        }
        public async Task<ResultModel> UpdateTask(string taskName, int order, Guid columnId, Guid taskId)
        {
            var task = await context.KanbanTasks.FindAsync(taskId);

            task.TaskName = taskName;
            task.ColumnId = columnId;
            task.Order = order;
            await context.SaveChangesAsync();
            
            return ResultModel.Ok();

        }
   
        public async Task<ResultModel> DeleteTask(Guid taskId)
        {
            var task = await context.KanbanTasks.FirstOrDefaultAsync(t => t.Id == taskId);

            context.KanbanTasks.Remove(task);
            await context.SaveChangesAsync();

            return ResultModel.Ok();
        }
        private async Task<int> GetNextOrderInColumn(Guid column, Guid userId)
        {
            var maxOrder = await context.KanbanTasks
                .Where(tp => tp.ColumnId == column && tp.UserId == userId)
                .MaxAsync(tp => (int?)tp.Order) ?? 0;
            return maxOrder + 1;
        }
    }
}
