using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TaskStatus = TodoListApp.Common.TaskStatus;

namespace TodoListApp.WebApi.Models.Entities;

/// <summary>
/// Represents a task item entity in the database.
/// </summary>
public class TaskItemEntity
{
    /// <summary>
    /// Gets or sets the ID of the task item.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the task item.
    /// </summary>
    [Required(ErrorMessage = "A title is required.")]
    [StringLength(100, ErrorMessage = "The title cannot be longer than 100 characters.")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the task item.
    /// </summary>
    [StringLength(500, ErrorMessage = "The description cannot be longer than 500 characters.")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the task item.
    /// </summary>
    public DateTime CreationDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the due date of the task item.
    /// </summary>
    public DateTime DueDate { get; set; } = DateTime.Now;

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
    /// Gets or sets the user assigned to the task item.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public IdentityUser? User { get; set; }

    /// <summary>
    /// Gets or sets the to-do list associated with the task item.
    /// </summary>
    [ForeignKey(nameof(TodoListId))]
    public TodoListEntity? TodoList { get; set; }

    /// <summary>
    /// Gets the collection of tags associated with the task item.
    /// </summary>
    public virtual ICollection<TagEntity>? Tags { get; private set; } = new List<TagEntity>();

    /// <summary>
    /// Gets the collection of comments associated with the task item.
    /// </summary>
    public virtual ICollection<CommentEntity>? Comments { get; private set; } = new List<CommentEntity>();

    public void SetTags(ICollection<TagEntity> tags)
    {
        this.Tags = tags;
    }

    public void SetComments(ICollection<CommentEntity> comments)
    {
        this.Comments = comments;
    }
}
