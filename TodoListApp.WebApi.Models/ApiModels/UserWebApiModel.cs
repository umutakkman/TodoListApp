namespace TodoListApp.WebApi.Models.ApiModels;

/// <summary>
/// Represents a user in the Web API.
/// </summary>
public class UserWebApiModel
{
    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets the collection of comments made by the user.
    /// </summary>
    public ICollection<CommentWebApiModel> Comments { get; } = new List<CommentWebApiModel>();

    /// <summary>
    /// Gets the collection of task items assigned to the user.
    /// </summary>
    public ICollection<TaskItemWebApiModel> TaskItems { get; } = new List<TaskItemWebApiModel>();
}
