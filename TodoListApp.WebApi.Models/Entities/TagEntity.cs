using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.Entities;

public class TagEntity
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "A name is required.")]
    [StringLength(50, ErrorMessage = "The name cannot be longer than 50 characters.")]
    public string Name { get; set; } = string.Empty;
}
