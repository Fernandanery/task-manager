namespace TaskManager.Application.Auth
{
    public interface IJwtTokenService
    {
        string GenerateToken(int userId, string email);
    }
}
