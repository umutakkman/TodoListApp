namespace TodoListApp.WebApi.Models.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
}
