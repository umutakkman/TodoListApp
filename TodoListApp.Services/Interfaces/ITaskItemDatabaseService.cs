using TodoListApp.WebApi.Models.Models;

namespace TodoListApp.Services.Interfaces;
public interface ITaskItemDatabaseService
{
    IQueryable<TaskItem> TaskItems { get; }

    void CreateTaskItem(TaskItem taskItem);

    void UpdateTaskItem(TaskItem taskItem);

    void DeleteTaskItem(TaskItem taskItem);
}
