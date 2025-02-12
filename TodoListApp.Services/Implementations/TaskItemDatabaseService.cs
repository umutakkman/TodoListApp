using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Models;

namespace TodoListApp.Services.Implementations;
public class TaskItemDatabaseService : ITaskItemDatabaseService
{
    private readonly TodoListDbContext context;

    public TaskItemDatabaseService(TodoListDbContext context)
    {
        this.context = context;
    }

    public IQueryable<TaskItem> TaskItems => this.context.TaskItems;

    public void CreateTaskItem(TaskItem taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        this.context.Add(taskItem);
        this.context.SaveChanges();
    }

    public void UpdateTaskItem(TaskItem taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        var existing = this.context.TaskItems.FirstOrDefault(x => x.Id == taskItem.Id);
        if (existing == null)
        {
            throw new InvalidOperationException("Task item not found");
        }

        this.context.Entry(existing).CurrentValues.SetValues(taskItem);
        this.context.SaveChanges();
    }

    public void DeleteTaskItem(TaskItem taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        var existing = this.context.TaskItems.FirstOrDefault(x => x.Id == taskItem.Id);
        if (existing == null)
        {
            throw new InvalidOperationException("Task item not found");
        }

        this.context.Remove(taskItem);
        this.context.SaveChanges();
    }
}
