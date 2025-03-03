using Core.DTO.TaskDTO;
using Core.ResultModels;

namespace Aplication.Services.Task
{
    public interface ITaskKanbanService
    {
        Task<List<TaskKanbanOrderDto>?> GetUserTasks(Guid userId);
        Task<ResultModelObject<TaskKanbanOrderDto>> CreateUserTasks(TaskKanbanDto request, Guid userId);
        Task<ResultModelObject<TaskKanbanOrderDto>> UpdateTask(Guid userId, TaskKanbanUpdateDto request);
        Task<ResultModel?> DeleteUserTasks(Guid id, Guid userId);
    }
}