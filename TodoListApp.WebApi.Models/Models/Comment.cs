using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.WebApi.Models.Models;

[NotMapped]
public class Comment
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public int TaskItemId { get; set; }

    public int? UserId { get; set; }

    public TaskItem TaskItem { get; set; } = null!;

    public User? User { get; set; }
}
