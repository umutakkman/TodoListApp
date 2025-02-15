using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Interfaces;
public interface ITaskItemDatabaseService
{
    IQueryable<TaskItemEntity> TaskItems { get; }

    void CreateTaskItem(TaskItemEntity taskItem);

    void UpdateTaskItem(TaskItemEntity taskItem);

    void DeleteTaskItem(TaskItemEntity taskItem);
}
