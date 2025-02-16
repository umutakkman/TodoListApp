using TaskStatus = TodoListApp.Common.TaskStatus;

namespace TodoListApp.WebApi.Models.ApiModels;

/// <summary>
/// Represents a task item in the Web API.
/// </summary>
public class TaskItemWebApiModel
{
    /// <summary>
    /// Gets or sets the ID of the task item.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the task item.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the task item.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the task item.
    /// </summary>
    public DateTime CreationDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the due date of the task item.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Gets or sets the status of the task item.
    /// </summary>
    public TaskStatus Status { get; set; } = TaskStatus.NotStarted;

    /// <summary>
    /// Gets or sets the ID of the user assigned to the task item.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the to-do list associated with the task item.
    /// </summary>
    public int TodoListId { get; set; }

    /// <summary>
    /// Gets or sets the collection of tags associated with the task item.
    /// </summary>
    public ICollection<TagWebApiModel>? Tags { get; set; } = new List<TagWebApiModel>();

    /// <summary>
    /// Gets or sets the collection of comments associated with the task item.
    /// </summary>
    public ICollection<CommentWebApiModel>? Comments { get; set; } = new List<CommentWebApiModel>();
}
