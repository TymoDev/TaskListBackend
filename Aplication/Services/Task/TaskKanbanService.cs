using Aplication.Core.Models.TaskModel;
using Core.DTO.TaskDTO;
using Core.Interfaces.Logging;
using Core.ResultModels;
using Persistance.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services.Task
{
    public class TaskKanbanService : ITaskKanbanService
    {
        private readonly ITaskKanbanRepository repository;
        private readonly IAppLogger logger;

        public TaskKanbanService(ITaskKanbanRepository repository, IAppLogger logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<List<TaskKanbanOrderDto>> GetUserTasks(Guid userId)
        {
            logger.Information($"[TaskKanbanService] Fetching Kanban tasks for user {userId}...");

            var response = await repository.GetUserTasks(userId);

            logger.Information($"[TaskKanbanService] Retrieved {response.Count} Kanban tasks for user {userId}.");
            return response;
        }

        public async Task<ResultModelObject<TaskKanbanOrderDto>> CreateUserTasks(TaskKanbanDto request, Guid userId)
        {
            logger.Information($"[TaskKanbanService] Creating Kanban task for user {userId}. Task Name: \"{request.taskName}\", Column: \"{request.column}\"...");

            var response = await repository.CreateTask(request.taskName, request.column, userId);

            if (response == null)
            {
                logger.Error($"[TaskKanbanService] Failed to create Kanban task for user {userId}. Task Name: \"{request.taskName}\".");
                return ResultModelObject<TaskKanbanOrderDto>.Error("Task creation failed.");
            }

            logger.Information($"[TaskKanbanService] Successfully created Kanban task {response.taskId} for user {userId}.");
            return ResultModelObject<TaskKanbanOrderDto>.Ok(response);
        }

        public async Task<ResultModel?> DeleteUserTasks(Guid id, Guid userId)
        {
            logger.Information($"[TaskKanbanService] Attempting to delete Kanban task {id} for user {userId}...");

            var userTasks = await repository.GetUserTasks(userId);
            var isUserTask = userTasks.FirstOrDefault(ut => ut.taskId == id);
            if (isUserTask == null)
            {
                logger.Warning($"[TaskKanbanService] Task {id} not found for user {userId}. Deletion aborted.");
                return ResultModel.Error("User does not have this task.");
            }

            var response = await repository.DeleteTask(id);
            if (response == null)
            {
                logger.Error($"[TaskKanbanService] Failed to delete Kanban task {id} for user {userId}.");
                return null;
            }

            logger.Information($"[TaskKanbanService] Successfully deleted Kanban task {id} for user {userId}.");
            return ResultModel.Ok();
        }
    }
}
