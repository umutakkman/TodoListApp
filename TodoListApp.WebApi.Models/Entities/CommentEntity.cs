using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TodoListApp.WebApi.Models.Entities;

public class CommentEntity
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "A comment is required.")]
    [StringLength(500, ErrorMessage = "The comment cannot be longer than 500 characters.")]
    public string Text { get; set; } = string.Empty;

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public int TaskItemId { get; set; }

    public string? UserId { get; set; }

    [ForeignKey(nameof(TaskItemId))]
    public virtual TaskItemEntity TaskItem { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public virtual IdentityUser? User { get; set; }
}
