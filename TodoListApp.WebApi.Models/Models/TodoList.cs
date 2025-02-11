namespace TodoListApp.WebApi.Models.Models
{
    public class TodoList
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<TaskItem> TaskItems { get; } = new List<TaskItem>();
    }
}
