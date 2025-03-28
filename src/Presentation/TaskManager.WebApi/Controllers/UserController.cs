using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.SharedKernel.Constants;

namespace TaskManager.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation(Miscellaneous.GetAllUsers);

            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation(Miscellaneous.GetUserById, id);

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning(Miscellaneous.UserByIdNotFound, id);
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto userDto)
        {
            _logger.LogInformation(Miscellaneous.CreateUserRequest, userDto);

            var user = _mapper.Map<User>(userDto);
            var result = await _userService.CreateUserAsync(user);

            _logger.LogInformation(Miscellaneous.UserCreatedSuccessfully, result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto updatedUser)
        {
            _logger.LogInformation(Miscellaneous.UpdateUserRequest, id, updatedUser);

            var user = await _userService.UpdateUserAsync(id, updatedUser);
            if (user == null)
            {
                _logger.LogWarning(Miscellaneous.UserUpdateNotFound, id);
                return NotFound();
            }

            _logger.LogInformation(Miscellaneous.UserUpdatedSuccessfully, id);
            return Ok(user);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation(Miscellaneous.DeleteUserRequest, id);

            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                _logger.LogWarning(Miscellaneous.UserDeleteNotFound, id);
                return NotFound();
            }

            _logger.LogInformation(Miscellaneous.UserDeletedSuccessfully, id);
            return NoContent();
        }
    }
}