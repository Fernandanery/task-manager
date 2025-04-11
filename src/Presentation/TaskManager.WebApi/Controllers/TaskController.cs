using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.SharedKernel.Constants;

namespace TaskManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController(TaskService taskService, IMapper mapper, ILogger<TaskController> logger) : ControllerBase
    {
        private readonly TaskService _taskService = taskService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<TaskController> _logger = logger;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            _logger.LogInformation(Miscellaneous.GetAllTasks);

            var tasks = await _taskService.GetAllTasks(cancellationToken);
            return Ok(tasks);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(Miscellaneous.GetTaskById, id);

            var task = await _taskService.GetTaskById(id, cancellationToken);

            if (task == null)
            {
                _logger.LogWarning(Miscellaneous.TaskByIdNotFound, id);
                return NotFound();
            }

            return Ok(task);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto taskDto, CancellationToken cancellationToken)
        {
            _logger.LogInformation(Miscellaneous.CreateTaskRequest, taskDto);

            var task = _mapper.Map<TaskItem>(taskDto);
            var result = await _taskService.CreateTask(task, cancellationToken);

            _logger.LogInformation(Miscellaneous.TaskCreatedSuccessfully, result.Id);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto taskDto, CancellationToken cancellationToken)
        {
            _logger.LogInformation(Miscellaneous.UpdateTaskRequest, id, taskDto);

            var result = await _taskService.UpdateTask(id, taskDto, cancellationToken);
            if (result == null)
            {
                _logger.LogWarning(Miscellaneous.TaskUpdateNotFound, id);
                return NotFound();
            }

            _logger.LogInformation(Miscellaneous.TaskUpdatedSuccessfully, id);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation(Miscellaneous.DeleteTaskRequest, id);

            var success = await _taskService.DeleteTask(id, cancellationToken);
            if (!success)
            {
                _logger.LogWarning(Miscellaneous.TaskDeleteNotFound, id);
                return NotFound();
            }

            _logger.LogInformation(Miscellaneous.TaskDeletedSuccessfully, id);
            return NoContent();
        }
    }
}