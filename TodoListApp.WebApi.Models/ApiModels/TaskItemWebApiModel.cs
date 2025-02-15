using TaskStatus = TodoListApp.Common.TaskStatus;

namespace TodoListApp.WebApi.Models.ApiModels;
public class TaskItemWebApiModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public DateTime DueDate { get; set; }

    public TaskStatus Status { get; set; } = TaskStatus.NotStarted;

    public int? UserId { get; set; }

    public int TodoListId { get; set; }

    public ICollection<TagWebApiModel>? Tags { get; set; } = new List<TagWebApiModel>();

    public ICollection<CommentWebApiModel>? Comments { get; } = new List<CommentWebApiModel>();
}
