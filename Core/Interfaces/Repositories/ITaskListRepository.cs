using Core.DTO.TaskDTO;

namespace DataAccess.Repositories.RepositoriesTb
{
    public interface ITaskListRepository
    {
        Task<TaskResponse?> CreateTask(Guid TaskId, string taskName, string taskStatus, Guid userId);
        Task<Guid?> DeleteTask(Guid id);
        Task<TaskResponse?> GetByIdTask(Guid TaskId);

        Task<List<TaskResponse?>> GetByUserTask(Guid userId);

        Task<List<TaskResponse>> GetTasks();
        Task<TaskResponse?> UpdateTask(Guid id, string taskName, string taskStatus);
    }
}