using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Implementations;
public class TaskItemDatabaseService : ITaskItemDatabaseService
{
    private readonly ApplicationDbContext context;

    public TaskItemDatabaseService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IQueryable<TaskItemEntity> TaskItems => this.context.TaskItem;

    public void CreateTaskItem(TaskItemEntity taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        _ = this.context.Add(taskItem);
        _ = this.context.SaveChanges();
    }

    public void UpdateTaskItem(TaskItemEntity taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        var existing = this.context.TaskItem.FirstOrDefault(x => x.Id == taskItem.Id);
        if (existing == null)
        {
            throw new InvalidOperationException("Task item not found");
        }

        this.context.Entry(existing).CurrentValues.SetValues(taskItem);
        _ = this.context.SaveChanges();
    }

    public void DeleteTaskItem(TaskItemEntity taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        var existing = this.context.TaskItem.FirstOrDefault(x => x.Id == taskItem.Id);
        if (existing == null)
        {
            throw new InvalidOperationException("Task item not found");
        }

        _ = this.context.Remove(taskItem);
        _ = this.context.SaveChanges();
    }
}
