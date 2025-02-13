using Core.DTO.TaskDTO;
using Core.Interfaces.Logging;
using Core.ResultModels;
using Core.ValidationModels.Task;
using Persistance.Repositories.Repositories;
using Persistance.Repositories.Repositories.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services.Task
{
    public class TaskKanbanColumnService : ITaskKanbanColumnService
    {
        private readonly ITaskKanbanColumnRepository repository;
        private readonly IAppLogger logger;

        public TaskKanbanColumnService(ITaskKanbanColumnRepository repository, IAppLogger logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<List<KanbanColumnDto>?> GetColumns(Guid userId)
        {
            logger.Information($"[TaskKanbanColumnService] Fetching Kanban columns for user: {userId}");

            var response = await repository.GetColumns(userId);
            if(response == null)
            {
                return null;
            }

            logger.Information($"[TaskKanbanColumnService] Retrieved {response.Count} Kanban columns for user: {userId}");
            return response;
        }

        public async Task<ResultModelObject<KanbanColumnDto>> CreateColumn(KanbanColumnDto request, Guid userId)
        {
            logger.Information($"[TaskKanbanColumnService] Creating new Kanban column. User: {userId}, Column Name: '{request.name}', Position: {request.position}");

            var response = await repository.CreateColumn(Guid.NewGuid(), userId, request.name, request.position);

            if (response == null)
            {
                logger.Error($"[TaskKanbanColumnService] Failed to create Kanban column for user: {userId}. Column Name: '{request.name}'");
                return ResultModelObject<KanbanColumnDto>.Error("Column creation failed.");
            }

            logger.Information($"[TaskKanbanColumnService] Successfully created Kanban column. ID: {response.id}, User: {userId}");
            return ResultModelObject<KanbanColumnDto>.Ok(response);
        }

        public async Task<ResultModel?> UpdateColumn(Guid userId,KanbanColumnDto request)
        {
            logger.Information($"Updating column with ID: {request.id} for user: {userId}");
            var userColumns = await repository.GetColumns(userId);
            if(userColumns == null)
            {

                return null;
            }

            var isUserColumnsExist = userColumns.FirstOrDefault(ut => ut.id == request.id);
            if (isUserColumnsExist == null)
            {
                logger.Warning($"Column with ID: {request.id} for user: {userId} not found");
                return ResultModel.Error("Sorry, but user don't have this column");
            }

            await repository.UpdateColumn(request.id,request.name,request.position);

            logger.Information($"Column with ID: {request.id} updated successfully");
            return ResultModel.Ok();

        }

        public async Task<ResultModel?> DeleteColumn(Guid id, Guid userId)
        {
            logger.Information($"[TaskKanbanColumnService] Request to delete Kanban column. Column ID: {id}, User: {userId}");

            var userColumns = await repository.GetColumns(userId);
            var isUserTask = userColumns.FirstOrDefault(ut => ut.id == id);

            if (isUserTask == null)
            {
                logger.Warning($"[TaskKanbanColumnService] Kanban column not found. Column ID: {id}, User: {userId}. Deletion aborted.");
                return ResultModel.Error("User does not have this column.");
            }

            var response = await repository.DeleteColumn(id);
            if (response == null)
            {
                logger.Error($"[TaskKanbanColumnService] Failed to delete Kanban column. Column ID: {id}, User: {userId}");
                return null;
            }

            logger.Information($"[TaskKanbanColumnService] Successfully deleted Kanban column. Column ID: {id}, User: {userId}");
            return ResultModel.Ok();
        }
    }
}
