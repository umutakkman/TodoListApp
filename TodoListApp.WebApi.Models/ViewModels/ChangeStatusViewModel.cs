namespace TodoListApp.WebApi.Models.ViewModels;

/// <summary>
/// View model for changing the status of a task.
/// </summary>
public class ChangeStatusViewModel
{
    /// <summary>
    /// Gets or sets the ID of the task.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the status of the task.
    /// </summary>
    public Common.TaskStatus TaskStatus { get; set; }
}
