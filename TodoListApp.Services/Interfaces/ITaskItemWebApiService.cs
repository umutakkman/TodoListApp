using TodoListApp.WebApi.Models.ApiModels;
using TaskStatus = TodoListApp.Common.TaskStatus;

namespace TodoListApp.Services.Interfaces;

/// <summary>
/// Interface for task item Web API service.
/// </summary>
public interface ITaskItemWebApiService
{
    /// <summary>
    /// Gets a task item asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the task item.</returns>
    Task<TaskItemWebApiModel> GetTaskItemAsync(int id);

    /// <summary>
    /// Creates a new task item asynchronously.
    /// </summary>
    /// <param name="taskItem">The task item to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created task item.</returns>
    Task<TaskItemWebApiModel> CreateTaskItemAsync(TaskItemWebApiModel taskItem);

    /// <summary>
    /// Deletes a task item asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteTaskItemAsync(int id);

    /// <summary>
    /// Updates a task item asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="taskItem">The task item to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated task item.</returns>
    Task<TaskItemWebApiModel> UpdateTaskItemAsync(int id, TaskItemWebApiModel taskItem);

    /// <summary>
    /// Gets the tasks assigned to a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of assigned tasks.</returns>
    Task<IEnumerable<TaskItemWebApiModel>> GetAssignedTasksAsync(string userId);

    /// <summary>
    /// Updates the status of a task item asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="status">The new status of the task item.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated task item.</returns>
    Task<TaskItemWebApiModel> UpdateTaskStatusAsync(int id, TaskStatus status);
}
