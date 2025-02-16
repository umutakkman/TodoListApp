using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TodoListApp.WebApi.Models.Entities;

/// <summary>
/// Represents a to-do list entity in the database.
/// </summary>
public class TodoListEntity
{
    /// <summary>
    /// Gets or sets the ID of the to-do list.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the to-do list.
    /// </summary>
    [Required(ErrorMessage = "A title is required.")]
    [StringLength(100, ErrorMessage = "The title cannot be longer than 100 characters.")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the to-do list.
    /// </summary>
    [StringLength(500, ErrorMessage = "The description cannot be longer than 500 characters.")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the owner of the to-do list.
    /// </summary>
    public string OwnerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner of the to-do list.
    /// </summary>
    [ForeignKey(nameof(OwnerId))]
    public virtual IdentityUser Owner { get; set; } = null!;

    /// <summary>
    /// Gets the collection of task items associated with the to-do list.
    /// </summary>
    public virtual ICollection<TaskItemEntity>? TaskItems { get; } = new List<TaskItemEntity>();
}
