using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.Entities;

public class StatusEntity
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "A status name is required.")]
    public string Name { get; set; } = string.Empty;

    public bool IsComplete { get; set; }
}
