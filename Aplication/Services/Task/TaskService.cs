using Aplication.Core.Models.TaskModel;
using Core.DTO.TaskDTO;
using Core.ResultModels;
using DataAccess.Repositories.RepositoriesTb;

namespace BusinessLogic.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository repository;

        public TaskService(ITaskRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<TaskResponse>> GetTasks()
        {
            var tasks = await repository.GetTasks();
            return tasks;
        }

        public async Task<TaskResponse?> GetTask(Guid id,Guid userId)
        {
            var userTasks = await repository.GetByUserTask(userId);
            var userTask = userTasks.FirstOrDefault(ut => ut.id == id);
            if (userTask == null)
            {
               return null; //add here result model with data in future
            }
            return userTask;
        }

        public async Task<List<TaskResponse?>> GetUserTasks(Guid userId)
        {
            var response = await repository.GetByUserTask(userId);
            if (response == null)
            {
                return null;
            }
            return response;
        }

        public async Task<ResultModelObject<TaskResponse>> CreateTask(TaskRequest request,Guid userId)
        {

            var response = TaskModel.Create(request.TaskName, request.TaskStatus);
            if (!response.Success)
            {
                return ResultModelObject<TaskResponse>.Error(response.ErrorMessage);
            }

            var serviceResponce = await repository.CreateTask(Guid.NewGuid(), request.TaskName, request.TaskStatus, userId);
            if (response == null)
            {
                return ResultModelObject<TaskResponse>.Error("Bad Request");
            }

            var taskResponse = new TaskResponse(serviceResponce.id, request.TaskName, request.TaskStatus);

            return ResultModelObject<TaskResponse>.Ok(taskResponse);
        }

        public async Task<ResultModelObject<TaskResponse?>> UpdateTask(Guid id,Guid userId, TaskRequest request)
        {
            var userTasks = await repository.GetByUserTask(userId);
            var isUserTaskExist = userTasks.FirstOrDefault(ut => ut.id == id);
            if (isUserTaskExist == null)
            {
                return ResultModelObject<TaskResponse?>.Error("Sorry, but user don't have this task");
            }
            var response = TaskModel.Create(request.TaskName, request.TaskStatus);
            if (!response.Success)
            {
                return ResultModelObject<TaskResponse?>.Error(response.ErrorMessage);
            }

            var result = await repository.UpdateTask(id, request.TaskName, request.TaskStatus);
            if (result == null)
            {
                return null;
            }
            return ResultModelObject<TaskResponse?>.Ok(result);
        }

        public async Task<ResultModel?> DeleteTask(Guid id, Guid userId)
        {
            var userTasks = await repository.GetByUserTask(userId);
            var isUserTask = userTasks.FirstOrDefault(ut => ut.id == id);
            if (isUserTask == null)
            {
                return ResultModel.Error("Sorry, but user don't have this task");
            }
            var result = await repository.DeleteTask(id);
            if (result == null)
            {
                return null;
            }
            return ResultModel.Ok();
        }

    }
}
