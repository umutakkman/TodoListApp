using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.WebApi.Models.Models;

[NotMapped]
public class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<TaskItem> TaskItems { get; } = new List<TaskItem>();
}
