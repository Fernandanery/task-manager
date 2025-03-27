using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Auth;
using TaskManager.Domain.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserRepository userRepository, IJwtTokenService jwtTokenService) : ControllerBase
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);

        if (user == null || user.Password != loginDto.Password)
            return Unauthorized("Credenciais inválidas.");

        var token = _jwtTokenService.GenerateToken(user.Id, user.Email);
        return Ok(new LoginResponseDto { Token = token });
    }
}