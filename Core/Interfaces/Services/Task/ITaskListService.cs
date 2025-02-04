using Core.DTO.TaskDTO;
using Core.ResultModels;

namespace BusinessLogic.Services
{
    public interface ITaskListService
    {
        Task<ResultModelObject<TaskResponse>> CreateTask(TaskRequest request, Guid userId);
        Task<ResultModel?> DeleteTask(Guid id, Guid userId);
        Task<TaskResponse?> GetTask(Guid id, Guid userId);
        Task<List<TaskResponse>> GetTasks();

        Task<List<TaskResponse?>> GetUserTasks(Guid userId);
        Task<ResultModelObject<TaskResponse?>> UpdateTask(Guid id, Guid userId, TaskRequest request);
    }
}