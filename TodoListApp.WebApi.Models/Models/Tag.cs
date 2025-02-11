namespace TodoListApp.WebApi.Models.Models;

public class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
}
