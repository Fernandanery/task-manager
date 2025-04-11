using Mapster;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IUserRepository _userRepository;

        public TaskService(ITaskRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<List<TaskItem>> GetAllTasks(CancellationToken cancellationToken)
        {

            return await _repository.GetAllTasksAsync(cancellationToken);
        }

        public async Task<TaskItem?> GetTaskById(int id, CancellationToken cancellationToken)
        {
            return await _repository.GetTaskByIdAsync(id, cancellationToken);
        }

        public async Task<TaskItem> CreateTask(TaskItem task, CancellationToken cancellationToken)
        {
            return await _repository.CreateTaskAsync(task, cancellationToken);
        }

        public async Task<TaskItem?> UpdateTask(int id, UpdateTaskDto taskDto, CancellationToken cancellationToken)
        {
            var existingTask = await _repository.GetTaskByIdAsync(id, cancellationToken);
            if (existingTask is null) return null;

            if (!string.IsNullOrEmpty(taskDto.Title))
                existingTask.Title = taskDto.Title;

            if (!string.IsNullOrEmpty(taskDto.Description))
                existingTask.Description = taskDto.Description;

            if (taskDto.IsCompleted.HasValue)
                existingTask.IsCompleted = taskDto.IsCompleted.Value;

            if (taskDto.UserId.HasValue)
            {
                var user = await _userRepository.GetUserByIdAsync(taskDto.UserId.Value, cancellationToken);
                if (user != null)
                {
                    existingTask.UserId = user.Id;
                }
            }

            await _repository.UpdateTaskAsync(id, existingTask, cancellationToken);
            return existingTask;
        }

        public async Task<bool> DeleteTask(int id, CancellationToken cancellationToken)
        {
            return await _repository.DeleteTaskAsync(id, cancellationToken);
        }

        public async Task<List<TaskItem>> GetPendingTasksAsync()
        {
            return await _repository.GetPendingTasksAsync();
        }
    }
}
