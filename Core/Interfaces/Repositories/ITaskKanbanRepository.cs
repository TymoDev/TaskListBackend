using Core.DTO.TaskDTO;
using Core.ResultModels;

namespace Persistance.Repositories.Repositories
{
    public interface ITaskKanbanRepository
    {
        Task<List<TaskKanbanOrderDto>> GetUserTasks(Guid userId);
        Task<TaskKanbanOrderDto?> CreateTask(string taskName, Guid columnId, Guid userId);
        Task<ResultModel> UpdateTask(string taskName, int order, Guid columnId, Guid taskId);
        Task<ResultModel> DeleteTask(Guid taskId);
    }
}