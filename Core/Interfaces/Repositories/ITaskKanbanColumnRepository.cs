using Core.DTO.TaskDTO;
using Core.ResultModels;

namespace Persistance.Repositories.Repositories.Tasks
{
    public interface ITaskKanbanColumnRepository
    {
        Task<KanbanColumnDto?> CreateColumn(Guid id, Guid userId, string name, int position);
        Task<ResultModel> UpdateColumn(Guid id, string name, int position);
        Task<ResultModel?> DeleteColumn(Guid taskId);
        Task<List<KanbanColumnDto>?> GetColumns(Guid userId);
    }
}