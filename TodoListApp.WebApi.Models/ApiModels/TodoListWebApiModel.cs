using TodoListApp.WebApi.Models.Models;

namespace TodoListApp.WebApi.Models.ApiModels;

public class TodoListWebApiModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public IEnumerable<TaskItemWebApiModel>? TaskItems { get; set; } = new List<TaskItemWebApiModel>();
}
