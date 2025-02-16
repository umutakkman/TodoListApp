namespace TodoListApp.WebApi.Models.ApiModels;

/// <summary>
/// Represents a tag in the Web API.
/// </summary>
public class TagWebApiModel
{
    /// <summary>
    /// Gets or sets the ID of the tag.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the tag.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets the collection of task items associated with the tag.
    /// </summary>
    public ICollection<TaskItemWebApiModel>? TaskItems { get; } = new List<TaskItemWebApiModel>();
}
