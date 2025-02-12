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

        public int? UserId { get; set; }

        public int TodoListId { get; set; }

        public User? User { get; set; }

        public TodoList? TodoList { get; set; }

        public ICollection<Tag>? Tags { get; set; } = new List<Tag>();

        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
    }
}
