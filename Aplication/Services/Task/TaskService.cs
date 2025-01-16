using Aplication.Core.Models.TaskModel;
using Core.DTO.TaskDTO;
using Core.Interfaces.Logging;
using Core.ResultModels;
using DataAccess.Repositories.RepositoriesTb;

namespace BusinessLogic.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository repository;
        private readonly IAppLogger logger;

        public TaskService(ITaskRepository repository,IAppLogger logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<List<TaskResponse>> GetTasks()
        {
            logger.Information("Fetching all tasks");
            var tasks = await repository.GetTasks();
            logger.Information($"Fetched {tasks.Count} tasks");
            return tasks;
        }

        public async Task<TaskResponse?> GetTask(Guid id, Guid userId)
        {
            logger.Information($"Fetching task with ID: {id} for user: {userId}");
            var userTasks = await repository.GetByUserTask(userId);
            var userTask = userTasks.FirstOrDefault(ut => ut.id == id);
            if (userTask == null)
            {
                logger.Warning($"Task with ID: {id} for user: {userId} not found");
                return null;
            }
            logger.Information($"Task with ID: {id} fetched successfully");
            return userTask;
        }

        public async Task<List<TaskResponse?>> GetUserTasks(Guid userId)
        {
            logger.Information($"Fetching tasks for user: {userId}");
            var response = await repository.GetByUserTask(userId);
            if (response == null)
            {
                logger.Warning($"No tasks found for user: {userId}");
                return null;
            }
            logger.Information($"Fetched {response.Count} tasks for user: {userId}");
            return response;
        }

        public async Task<ResultModelObject<TaskResponse>> CreateTask(TaskRequest request, Guid userId)
        {
            logger.Information($"Creating task for user: {userId} with name: {request.TaskName}");

            var response = TaskModel.Create(request.TaskName, request.TaskStatus);
            if (!response.Success)
            {
                logger.Error($"Task creation failed: {response.ErrorMessage}");
                return ResultModelObject<TaskResponse>.Error(response.ErrorMessage);
            }

            var serviceResponse = await repository.CreateTask(Guid.NewGuid(), request.TaskName, request.TaskStatus, userId);
            if (serviceResponse == null)
            {
                logger.Error("Task creation failed: Bad Request");
                return ResultModelObject<TaskResponse>.Error("Bad Request");
            }

            var taskResponse = new TaskResponse(serviceResponse.id, request.TaskName, request.TaskStatus);
            logger.Information($"Task created successfully with ID: {serviceResponse.id}");

            return ResultModelObject<TaskResponse>.Ok(taskResponse);
        }

        public async Task<ResultModelObject<TaskResponse?>> UpdateTask(Guid id, Guid userId, TaskRequest request)
        {
            logger.Information($"Updating task with ID: {id} for user: {userId}");
            var userTasks = await repository.GetByUserTask(userId);
            var isUserTaskExist = userTasks.FirstOrDefault(ut => ut.id == id);
            if (isUserTaskExist == null)
            {
                logger.Warning($"Task with ID: {id} for user: {userId} not found");
                return ResultModelObject<TaskResponse?>.Error("Sorry, but user don't have this task");
            }

            var response = TaskModel.Create(request.TaskName, request.TaskStatus);
            if (!response.Success)
            {
                logger.Error($"Task update failed: {response.ErrorMessage}");
                return ResultModelObject<TaskResponse?>.Error(response.ErrorMessage);
            }

            var result = await repository.UpdateTask(id, request.TaskName, request.TaskStatus);
            if (result == null)
            {
                logger.Error("Task update failed: Repository returned null");
                return null;
            }

            logger.Information($"Task with ID: {id} updated successfully");
            return ResultModelObject<TaskResponse?>.Ok(result);
        }

        public async Task<ResultModel?> DeleteTask(Guid id, Guid userId)
        {
            logger.Information($"Deleting task with ID: {id} for user: {userId}");
            var userTasks = await repository.GetByUserTask(userId);
            var isUserTask = userTasks.FirstOrDefault(ut => ut.id == id);
            if (isUserTask == null)
            {
                logger.Warning($"Task with ID: {id} for user: {userId} not found");
                return ResultModel.Error("Sorry, but user don't have this task");
            }

            var result = await repository.DeleteTask(id);
            if (result == null)
            {
                logger.Error("Task deletion failed: Repository returned null");
                return null;
            }

            logger.Information($"Task with ID: {id} deleted successfully");
            return ResultModel.Ok();
        }

    }
}
