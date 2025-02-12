using Core.DTO.TaskDTO;
using Core.ResultModels;

namespace Aplication.Services.Task
{
    public interface ITaskKanbanColumnService
    {
        Task<ResultModelObject<KanbanColumnDto>> CreateColumn(KanbanColumnDto request, Guid userId);
        Task<ResultModel?> DeleteColumn(Guid id, Guid userId);
        Task<ResultModel?> UpdateColumn(Guid userId, KanbanColumnDto request);
        Task<List<KanbanColumnDto>> GetColumns(Guid userId);
    }
}