using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.WebApi.Models.Entities;

public class TaskItemEntity
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "A title is required.")]
    [StringLength(100, ErrorMessage = "The title cannot be longer than 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "The description cannot be longer than 500 characters.")]
    public string? Description { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public DateTime DueDate { get; set; } = DateTime.Now;

    public int StatusId { get; set; }

    public int UserId { get; set; }

    public int TodoListId { get; set; }

    [ForeignKey(nameof(StatusId))]
    public StatusEntity Status { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public UserEntity User { get; set; } = null!;

    [ForeignKey(nameof(TodoListId))]
    public TodoListEntity TodoList { get; set; } = null!;

    public virtual ICollection<TagEntity> Tags { get; } = new List<TagEntity>();

    public virtual ICollection<CommentEntity> Comments { get; } = new List<CommentEntity>();
}
