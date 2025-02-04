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

namespace Persistance.Repositories.Repositories
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
             .Where(tp => tp.UserId == userId)
             .Select(tp => new TaskKanbanOrderDto(tp.Id, tp.TaskName, tp.Column, tp.Order, tp.UserId))
             .ToListAsync();
            return kanbanTasks;
        }


        public async Task<TaskKanbanOrderDto?> CreateTask(string taskName, string column, Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            var order = await GetNextOrderInColumn(column, userId);
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
            
            return new TaskKanbanOrderDto(newKanbanTask.Id, newKanbanTask.TaskName, newKanbanTask.Column, order, userId);
        }

        private async Task<int> GetNextOrderInColumn(string column, Guid userId)
        {
            var maxOrder = await context.KanbanTasks
                .Where(tp => tp.Column == column && tp.UserId == userId)
                .MaxAsync(tp => (int?)tp.Order) ?? 0;
            return maxOrder + 1;
        }
        public async Task<ResultModel?> DeleteTask(Guid taskId)
        {
            var task = await context.KanbanTasks.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null) return null;

            var position = await context.KanbanTasks.FirstOrDefaultAsync(tp => tp.Id == taskId);
            if (position != null)
            {
                context.KanbanTasks.Remove(position);
            }

            context.KanbanTasks.Remove(task);
            await context.SaveChangesAsync();

            return ResultModel.Ok();
        }
    }
}
