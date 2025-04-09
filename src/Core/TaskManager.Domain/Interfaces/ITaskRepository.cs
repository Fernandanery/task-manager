using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetAllTasksAsync(CancellationToken cancellationToken);
        Task<TaskItem?> GetTaskByIdAsync(int id, CancellationToken cancellationToken);
        Task<TaskItem> CreateTaskAsync(TaskItem task, CancellationToken cancellationToken);
        Task<TaskItem?> UpdateTaskAsync(int id, TaskItem task, CancellationToken cancellationToken);
        Task<bool> DeleteTaskAsync(int id, CancellationToken cancellationToken);
        Task<List<TaskItem>> GetPendingTasksAsync();
    }
}
