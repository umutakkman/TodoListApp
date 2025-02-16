using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.Entities;

/// <summary>
/// Represents a tag entity in the database.
/// </summary>
public class TagEntity
{
    /// <summary>
    /// Gets or sets the ID of the tag.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the tag.
    /// </summary>
    [Required(ErrorMessage = "A name is required.")]
    [StringLength(50, ErrorMessage = "The name cannot be longer than 50 characters.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the collection of task items associated with the tag.
    /// </summary>
    public virtual ICollection<TaskItemEntity> TaskItems { get; } = new List<TaskItemEntity>();
}
