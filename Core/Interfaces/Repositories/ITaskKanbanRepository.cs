using Core.DTO.TaskDTO;
using Core.ResultModels;

namespace Persistance.Repositories.Repositories
{
    public interface ITaskKanbanRepository
    {
        Task<List<TaskKanbanOrderDto>> GetUserTasks(Guid userId);
        Task<TaskKanbanOrderDto?> CreateTask(string taskName, string column, Guid userId);
        Task<ResultModel?> DeleteTask(Guid taskId);
    }
}