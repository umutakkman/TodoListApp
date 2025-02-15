using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskStatus = TodoListApp.Common.TaskStatus;

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

    public TaskStatus Status { get; set; } = TaskStatus.NotStarted;

    public int? UserId { get; set; }

    public int TodoListId { get; set; }

    [ForeignKey(nameof(UserId))]
    public UserEntity? User { get; set; }

    [ForeignKey(nameof(TodoListId))]
    public TodoListEntity? TodoList { get; set; }

    public virtual ICollection<TagEntity>? Tags { get; set; } = new List<TagEntity>();

    public virtual ICollection<CommentEntity>? Comments { get; } = new List<CommentEntity>();
}
