using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.Entities;

public class UserEntity
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "A username is required.")]
    [StringLength(30, ErrorMessage = "The username cannot be longer than 30 characters.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "A password is required.")]
    public string Password { get; set; } = string.Empty;

    public virtual ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();

    public virtual ICollection<TaskItemEntity> TaskItems { get; set; } = new List<TaskItemEntity>();
}
