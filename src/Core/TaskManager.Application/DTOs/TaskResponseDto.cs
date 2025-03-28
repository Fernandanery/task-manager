namespace TaskManager.Application.DTOs
{
    public record TaskResponseDto(int Id, string Title, string Description, bool IsCompleted, DateTime CreatedAt, int? UserId, string? UserName, string? UserEmail);
}