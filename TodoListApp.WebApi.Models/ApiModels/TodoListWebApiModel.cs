namespace TodoListApp.WebApi.Models.ApiModels;

/// <summary>
/// Represents a to-do list in the Web API.
/// </summary>
public class TodoListWebApiModel
{
    /// <summary>
    /// Gets or sets the ID of the to-do list.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the to-do list.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the to-do list.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the ID of the owner of the to-do list.
    /// </summary>
    public string OwnerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of task items associated with the to-do list.
    /// </summary>
    public IEnumerable<TaskItemWebApiModel>? TaskItems { get; set; } = new List<TaskItemWebApiModel>();
}
