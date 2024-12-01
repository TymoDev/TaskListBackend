using Core.DTO.TaskDTO;
using Core.ResultModels;

namespace BusinessLogic.Services
{
    public interface ITaskService
    {
        Task<ResultModel> CreateTask(TaskRequest request, Guid userId);
        Task<ResultModel?> DeleteTask(Guid id, Guid userId);
        Task<TaskResponse?> GetTask(Guid id, Guid userId);
        Task<List<TaskResponse>> GetTasks();

        Task<List<TaskResponse?>> GetUserTasks(Guid userId);
        Task<ResultModel?> UpdateTask(Guid id, Guid userId, TaskRequest request);
    }
}