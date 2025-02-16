using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TodoListApp.WebApi.Models.Entities;

/// <summary>
/// Represents a comment entity in the database.
/// </summary>
public class CommentEntity
{
    /// <summary>
    /// Gets or sets the ID of the comment.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the text of the comment.
    /// </summary>
    [Required(ErrorMessage = "A comment is required.")]
    [StringLength(500, ErrorMessage = "The comment cannot be longer than 500 characters.")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creation date of the comment.
    /// </summary>
    public DateTime CreationDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets or sets the ID of the task item associated with the comment.
    /// </summary>
    public int TaskItemId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user who created the comment.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the task item associated with the comment.
    /// </summary>
    [ForeignKey(nameof(TaskItemId))]
    public virtual TaskItemEntity TaskItem { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user who created the comment.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual IdentityUser? User { get; set; }
}
