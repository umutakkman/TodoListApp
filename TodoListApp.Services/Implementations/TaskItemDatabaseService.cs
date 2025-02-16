using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Implementations;

/// <summary>
/// Service for managing task items in the database.
/// </summary>
public class TaskItemDatabaseService : ITaskItemDatabaseService
{
    private readonly ApplicationDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskItemDatabaseService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public TaskItemDatabaseService(ApplicationDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets the collection of task items.
    /// </summary>
    public IQueryable<TaskItemEntity> TaskItems => this.context.TaskItem;

    /// <summary>
    /// Creates a new task item.
    /// </summary>
    /// <param name="taskItem">The task item entity to create.</param>
    /// <exception cref="ArgumentNullException">Thrown when the task item is null.</exception>
    public void CreateTaskItem(TaskItemEntity taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);

        _ = this.context.Add(taskItem);
        _ = this.context.SaveChanges();
    }

    /// <summary>
    /// Updates an existing task item.
    /// </summary>
    /// <param name="taskItem">The task item entity to update.</param>
    /// <exception cref="ArgumentNullException">Thrown when the task item is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the task item is not found.</exception>
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

    /// <summary>
    /// Deletes a task item.
    /// </summary>
    /// <param name="taskItem">The task item entity to delete.</param>
    /// <exception cref="ArgumentNullException">Thrown when the task item is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the task item is not found.</exception>
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
