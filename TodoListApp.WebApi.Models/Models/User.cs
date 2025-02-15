using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.WebApi.Models.Models;

public class User
{
    [NotMapped]
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public ICollection<Comment> Comments { get; } = new List<Comment>();

    public ICollection<TaskItem> TaskItems { get; } = new List<TaskItem>();
}
