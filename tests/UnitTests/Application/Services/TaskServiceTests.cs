using Moq;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.UnitTests.Application.Services
{
    public class TaskServiceTests
    {
        private readonly TaskService _taskService;
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllTasks_ShouldReturnTasks()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem
                {
                    Id = 1,
                    Title = "Test Task",
                    Description = "Task description",
                    CreatedAt = DateTime.Now,
                    UserId = 1,
                    IsCompleted = false
                }
            };
            _taskRepositoryMock
                .Setup(r => r.GetAllTasksAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetAllTasks(CancellationToken.None);

            // Assert
            Assert.NotEmpty(result);
            Assert.Single(result);
            Assert.Equal("Test Task", result[0].Title);
        }

        [Fact]
        public async Task GetTaskById_ExistingId_ReturnsTask()
        {
            // Arrange
            var task = new TaskItem
            {
                Id = 1,
                Title = "Existing Task",
                Description = "Task description",
                CreatedAt = DateTime.Now,
                UserId = 1
            };
            _taskRepositoryMock
                .Setup(r => r.GetTaskByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(task);

            // Act
            var result = await _taskService.GetTaskById(1, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Existing Task", result!.Title);
        }

        [Fact]
        public async Task CreateTask_ShouldReturnCreatedTask()
        {
            // Arrange
            var newTask = new TaskItem
            {
                Title = "New Task",
                Description = "New description",
                CreatedAt = DateTime.Now,
                UserId = 1
            };

            _taskRepositoryMock
                .Setup(r => r.CreateTaskAsync(newTask, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newTask);

            // Act
            var result = await _taskService.CreateTask(newTask, CancellationToken.None);

            // Assert
            Assert.Equal("New Task", result.Title);
        }

        [Fact]
        public async Task UpdateTask_ExistingId_ShouldUpdateAndReturnTask()
        {
            // Arrange
            var updateDto = new UpdateTaskDto
            {
                Title = "Updated Task",
                Description = "Updated Desc"
            };

            var existingTask = new TaskItem
            {
                Id = 1,
                Title = "Old Title",
                Description = "Old Desc",
                CreatedAt = DateTime.Now,
                UserId = 1
            };

            _taskRepositoryMock
                .Setup(r => r.GetTaskByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingTask);

            _taskRepositoryMock
                .Setup(r => r.GetTaskByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingTask);

            // Act
            var result = await _taskService.UpdateTask(1, updateDto, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Task", result!.Title);
            Assert.Equal("Updated Desc", result.Description);
        }

        [Fact]
        public async Task DeleteTask_ValidId_ReturnsTrue()
        {
            // Arrange
            _taskRepositoryMock
                .Setup(r => r.DeleteTaskAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _taskService.DeleteTask(1, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetPendingTasksAsync_ShouldReturnList()
        {
            // Arrange
            var pendingTasks = new List<TaskItem>
            {
                new TaskItem
                {
                    Title = "Pending",
                    Description = "Pending Desc",
                    CreatedAt = DateTime.Now,
                    UserId = 1
                }
            };

            _taskRepositoryMock
                .Setup(r => r.GetPendingTasksAsync())
                .ReturnsAsync(pendingTasks);

            // Act
            var result = await _taskService.GetPendingTasksAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Pending", result[0].Title);
        }
    }
}