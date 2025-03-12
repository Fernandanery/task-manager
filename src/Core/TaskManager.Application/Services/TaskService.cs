using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TaskItem>> GetAllTasks()
        {
            return await _repository.GetAllTasksAsync();
        }

        public async Task<TaskItem?> GetTaskById(int id)
        {
            return await _repository.GetTaskByIdAsync(id);
        }

        public async Task<TaskItem> CreateTask(TaskItem task)
        {
            return await _repository.CreateTaskAsync(task);
        }

        public async Task<TaskItem?> UpdateTask(int id, TaskItem updatedTask)
        {
            return await _repository.UpdateTaskAsync(id, updatedTask);
        }

        public async Task<bool> DeleteTask(int id)
        {
            return await _repository.DeleteTaskAsync(id);
        }
    }
}
