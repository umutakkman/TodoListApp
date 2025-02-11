namespace TodoListApp.WebApi.Models.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public DateTime DueDate { get; set; } = DateTime.Now;

        public int StatusId { get; set; }

        public int UserId { get; set; }

        public int TodoListId { get; set; }

        public Status Status { get; set; } = null!;

        public User User { get; set; } = null!;

        public TodoList TodoList { get; set; } = null!;

        public ICollection<Tag> Tags { get; } = new List<Tag>();

        public ICollection<Comment> Comments { get; } = new List<Comment>();
    }
}
