using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskRepository(ApplicationDbContext context) : ITaskRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<TaskItem>> GetAllTasksAsync(CancellationToken cancellationToken)
        {
            return await _context.Tasks
                .Include(t => t.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Tasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task, CancellationToken cancellationToken)
        {
            if (task.UserId.HasValue)
            {
                var user = await _context.Users.FindAsync(task.UserId);
                if (user != null)
                {
                    task.UserId = user.Id;
                }
            }

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem?> UpdateTaskAsync(int id, TaskItem updatedTask, CancellationToken cancellationToken)
        {
            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask is null) return null;

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.IsCompleted = updatedTask.IsCompleted;

            if (updatedTask.UserId.HasValue)
            {
                var user = await _context.Users.FindAsync(updatedTask.UserId);
                if (user != null)
                {
                    existingTask.UserId = user.Id;
                }
            }

            await _context.SaveChangesAsync();
            return existingTask;
        }

        public async Task<bool> DeleteTaskAsync(int id, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task is null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TaskItem>> GetPendingTasksAsync()
        {
            return await _context.Tasks
                .Where(t => t.IsCompleted == false)
                .ToListAsync();
        }
    }
}
