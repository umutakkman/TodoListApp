namespace TodoListApp.WebApi.Models.ApiModels;

/// <summary>
/// Represents a comment in the Web API.
/// </summary>
public class CommentWebApiModel
{
    /// <summary>
    /// Gets or sets the ID of the comment.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the text of the comment.
    /// </summary>
    public string Text { get; set; } = null!;

    /// <summary>
    /// Gets or sets the creation date of the comment.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the ID of the task item associated with the comment.
    /// </summary>
    public int TaskItemId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user who created the comment.
    /// </summary>
    public string? UserId { get; set; }
}
