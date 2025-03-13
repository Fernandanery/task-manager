using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;

namespace TaskManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _taskService;
        private readonly IMapper _mapper;

        public TaskController(TaskService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService.GetAllTasks();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetTaskById(id);
            if (task == null) return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto taskDto)
        {
            var task = _mapper.Map<TaskItem>(taskDto);
            var result = await _taskService.CreateTask(task);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto taskDto)
        {
            var result = await _taskService.UpdateTask(id, taskDto);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _taskService.DeleteTask(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
