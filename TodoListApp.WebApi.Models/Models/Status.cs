namespace TodoListApp.WebApi.Models.Models;

public class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsComplete { get; set; }
}
