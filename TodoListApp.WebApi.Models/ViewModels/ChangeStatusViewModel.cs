namespace TodoListApp.WebApi.Models.ViewModels;
public class ChangeStatusViewModel
{
    public int Id { get; set; }

    public Common.TaskStatus TaskStatus { get; set; }
}
