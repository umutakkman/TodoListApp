namespace TodoListApp.WebApi.Models.ApiModels;

public class UserWebApiModel
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public ICollection<CommentWebApiModel> Comments { get; } = new List<CommentWebApiModel>();

    public ICollection<TaskItemWebApiModel> TaskItems { get; } = new List<TaskItemWebApiModel>();
}
