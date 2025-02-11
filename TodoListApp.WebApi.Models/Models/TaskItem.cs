using TaskStatus = TodoListApp.Common.TaskStatus;

namespace TodoListApp.WebApi.Models.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public DateTime DueDate { get; set; } = DateTime.Now;

        public TaskStatus Status { get; set; } = TaskStatus.NotStarted;

        public int UserId { get; set; }

        public int TodoListId { get; set; }

        public User User { get; set; } = null!;

        public TodoList TodoList { get; set; } = null!;

        public ICollection<Tag> Tags { get; } = new List<Tag>();

        public ICollection<Comment> Comments { get; } = new List<Comment>();
    }
}
