using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetPendingTasksAsync();

    }
}
