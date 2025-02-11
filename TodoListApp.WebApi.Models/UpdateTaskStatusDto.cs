using TaskStatus = TodoListApp.Common.TaskStatus;

namespace TodoListApp.WebApi.Models;
public class UpdateTaskStatusDto
{
    public TaskStatus Status { get; set; }
}
