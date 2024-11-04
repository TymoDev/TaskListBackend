using Core.DTO.TaskDTO;
using Core.ResultModels;

namespace BusinessLogic.Services
{
    public interface ITaskService
    {
        Task<ResultModel> CreateTask(TaskRequest request);
        Task<ResultModel?> DeleteTask(Guid id);
        Task<TaskResponse?> GetTask(Guid id);
        Task<List<TaskResponse>> GetTasks();
        Task<List<TaskResponse?>> GetUserTasks(Guid userId);
        Task<ResultModel?> UpdateTask(Guid id, TaskRequest request);
    }
}