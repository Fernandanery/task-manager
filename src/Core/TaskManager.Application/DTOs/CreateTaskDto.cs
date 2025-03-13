public class CreateTaskDto
{
    public required string Title { get; set; } = null!;
    public required string Description { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? UserId { get; set; }
}
