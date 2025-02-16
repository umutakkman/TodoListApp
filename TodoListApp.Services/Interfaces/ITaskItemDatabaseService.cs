using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Interfaces;

/// <summary>
/// Interface for task item database service.
/// </summary>
public interface ITaskItemDatabaseService
{
    /// <summary>
    /// Gets the collection of task items.
    /// </summary>
    IQueryable<TaskItemEntity> TaskItems { get; }

    /// <summary>
    /// Creates a new task item.
    /// </summary>
    /// <param name="taskItem">The task item entity to create.</param>
    void CreateTaskItem(TaskItemEntity taskItem);

    /// <summary>
    /// Updates an existing task item.
    /// </summary>
    /// <param name="taskItem">The task item entity to update.</param>
    void UpdateTaskItem(TaskItemEntity taskItem);

    /// <summary>
    /// Deletes a task item.
    /// </summary>
    /// <param name="taskItem">The task item entity to delete.</param>
    void DeleteTaskItem(TaskItemEntity taskItem);
}
