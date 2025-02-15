namespace TodoListApp.WebApi.Models.ApiModels;
public class TagWebApiModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<TaskItemWebApiModel>? TaskItems { get; } = new List<TaskItemWebApiModel>();
}
