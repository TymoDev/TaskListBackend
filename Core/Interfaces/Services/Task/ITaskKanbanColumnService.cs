using Core.DTO.TaskDTO;
using Core.ResultModels;

namespace Aplication.Services.Task
{
    public interface ITaskKanbanColumnService
    {
        Task<ResultModelObject<KanbanColumnDto>> CreateUserColumns(KanbanColumnDto request, Guid userId);
        Task<ResultModel?> DeleteUserColumns(Guid id, Guid userId);
        Task<List<KanbanColumnDto>> GetUserColumns(Guid userId);
    }
}