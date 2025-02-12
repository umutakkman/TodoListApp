namespace TodoListApp.WebApi.Models.ApiModels;
public class CommentWebApiModel
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public int TaskItemId { get; set; }

    public int? UserId { get; set; }
}
