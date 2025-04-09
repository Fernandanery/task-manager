using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Auth;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.SharedKernel.Constants;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IUserRepository userRepository, IJwtTokenService jwtTokenService, IPasswordHasher<User> passwordHasher) : ControllerBase
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(loginDto.Email, cancellationToken);

        if (user == null)
            return Unauthorized(Miscellaneous.InvalidCredentials);

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);


        if (result == PasswordVerificationResult.Failed)
            return Unauthorized(Miscellaneous.InvalidCredentials);

        var token = _jwtTokenService.GenerateToken(user.Id, user.Email);
        return Ok(new LoginResponseDto { Token = token });
    }
}