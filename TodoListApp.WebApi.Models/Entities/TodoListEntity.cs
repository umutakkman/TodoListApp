using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.Entities;

public class TodoListEntity
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "A title is required.")]
    [StringLength(100, ErrorMessage = "The title cannot be longer than 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "The description cannot be longer than 500 characters.")]
    public string Description { get; set; } = string.Empty;

    public virtual ICollection<TaskItemEntity>? TaskItems { get; set; } = new List<TaskItemEntity>();
}
