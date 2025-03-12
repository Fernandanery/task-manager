using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Domain.Entities
{
    public class TaskItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UserId { get; set; }
        public User? User { get; private set; }
    }
}
