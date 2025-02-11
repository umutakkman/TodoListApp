namespace TodoListApp.WebApi.Models;

public class TodoListWebApiModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }
}
