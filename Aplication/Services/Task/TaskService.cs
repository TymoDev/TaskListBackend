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

        public async Task<TaskResponse?> GetTask(Guid id)
        {
            var response = await repository.GetByIdTask(id);
            if (response == null)
            {
                return null;
            }
            return response;
        }

       /* public async Task<List<TaskResponse?>> GetUserTasks(Guid userId)
        {
            var response = await repository.GetByUserTask(userId);
            if (response == null)
            {
                return null;
            }
            return response;
        }*/

        public async Task<ResultModel> CreateTask(TaskRequest request)
        {
            /* Guid userId;

             if (!Guid.TryParse(request.UserId, out userId))
             {
                 return ResultModel.Error("Incorrect format");
             }*/

            var response = TaskModel.Create(request.TaskName, request.TaskStatus);
            if (!response.Success)
            {
                return ResultModel.Error(response.ErrorMessage);
            }

            var responseService = await repository.CreateTask(Guid.NewGuid(), request.TaskName, request.TaskStatus/* userId*/);
            if (responseService == null)
            {
                return ResultModel.Error("BadRequest");
            }

            return ResultModel.Ok();
        }

        public async Task<ResultModel?> UpdateTask(Guid id, TaskRequest request)
        {
            var response = TaskModel.Create(request.TaskName, request.TaskStatus);
            if (!response.Success)
            {
                return ResultModel.Error(response.ErrorMessage);
            }

            var result = await repository.UpdateTask(id, request.TaskName, request.TaskStatus);
            if (result == null)
            {
                return null;
            }
            return ResultModel.Ok();
        }

        public async Task<ResultModel?> DeleteTask(Guid id)
        {
            var result = await repository.DeleteTask(id);
            if (result == null)
            {
                return null;
            }
            return ResultModel.Ok();
        }
    }
}
