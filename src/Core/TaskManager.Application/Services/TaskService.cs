using Mapster;
using TaskManager.Application.DTOs;
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

        public async Task<TaskItem?> UpdateTask(int id, UpdateTaskDto taskDto)
        {
            var existingTask = await _repository.GetTaskByIdAsync(id);
            if (existingTask is null) return null;

            if (!string.IsNullOrEmpty(taskDto.Title))
                existingTask.Title = taskDto.Title;

            if (!string.IsNullOrEmpty(taskDto.Description))
                existingTask.Description = taskDto.Description;

            if (taskDto.IsCompleted.HasValue)
                existingTask.IsCompleted = taskDto.IsCompleted.Value;

            if (taskDto.UserId.HasValue)
            {
                var user = await _repository.GetTaskByIdAsync(taskDto.UserId.Value);
                if (user != null)
                {
                    existingTask.UserId = user.Id;
                }
            }

            await _repository.UpdateTaskAsync(id, existingTask);
            return existingTask;
        }
        public async Task<bool> DeleteTask(int id)
        {
            return await _repository.DeleteTaskAsync(id);
        }
    }
}
