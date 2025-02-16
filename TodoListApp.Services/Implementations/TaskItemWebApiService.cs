using System.Net.Http.Json;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;
using TodoListApp.WebApi.Models.ViewModels;
using TaskStatus = TodoListApp.Common.TaskStatus;

namespace TodoListApp.Services.Implementations;

/// <summary>
/// Service for managing task items via Web API.
/// </summary>
public class TaskItemWebApiService : ITaskItemWebApiService
{
    private readonly HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskItemWebApiService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public TaskItemWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /// <summary>
    /// Gets a task item asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the task item.</returns>
    public async Task<TaskItemWebApiModel> GetTaskItemAsync(int id)
    {
        var response = await this.httpClient.GetFromJsonAsync<TaskItemWebApiModel>($"api/TaskItem/{id}");
        ArgumentNullException.ThrowIfNull(response);

        return response;
    }

    /// <summary>
    /// Creates a new task item asynchronously.
    /// </summary>
    /// <param name="taskItem">The task item to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created task item.</returns>
    public async Task<TaskItemWebApiModel> CreateTaskItemAsync(TaskItemWebApiModel taskItem)
    {
        var response = await this.httpClient.PostAsJsonAsync("api/TaskItem", taskItem);

        _ = response.EnsureSuccessStatusCode();

        var createdtaskItem = await response.Content.ReadFromJsonAsync<TaskItemWebApiModel>();
        ArgumentNullException.ThrowIfNull(createdtaskItem);
        return createdtaskItem;
    }

    /// <summary>
    /// Deletes a task item asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteTaskItemAsync(int id)
    {
        var uri = new Uri($"api/TaskItem/{id}", UriKind.Relative);
        var response = await this.httpClient.DeleteAsync(uri);
        _ = response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Updates a task item asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="taskItem">The task item to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated task item.</returns>
    public async Task<TaskItemWebApiModel> UpdateTaskItemAsync(int id, TaskItemWebApiModel taskItem)
    {
        var response = await this.httpClient.PutAsJsonAsync($"api/TaskItem/{id}", taskItem);
        _ = response.EnsureSuccessStatusCode();
        var updatedtaskItem = await response.Content.ReadFromJsonAsync<TaskItemWebApiModel>();
        ArgumentNullException.ThrowIfNull(updatedtaskItem);
        return updatedtaskItem;
    }

    /// <summary>
    /// Gets the tasks assigned to a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of assigned tasks.</returns>
    public async Task<IEnumerable<TaskItemWebApiModel>> GetAssignedTasksAsync(string userId)
    {
        var response = await this.httpClient.GetFromJsonAsync<IEnumerable<TaskItemWebApiModel>>($"api/TaskItem/assigned/{userId}");
        ArgumentNullException.ThrowIfNull(response);
        return response;
    }

    /// <summary>
    /// Updates the status of a task item asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="status">The new status of the task item.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated task item.</returns>
    public async Task<TaskItemWebApiModel> UpdateTaskStatusAsync(int id, TaskStatus status)
    {
        var model = new ChangeStatusViewModel
        {
            Id = id,
            TaskStatus = status,
        };

        var response = await this.httpClient.PutAsJsonAsync($"api/TaskItem/status/{id}", model);

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return await this.GetTaskItemAsync(id);
        }

        _ = response.EnsureSuccessStatusCode();
        var updatedTask = await response.Content.ReadFromJsonAsync<TaskItemWebApiModel>();
        ArgumentNullException.ThrowIfNull(updatedTask);
        return updatedTask;
    }

    /// <summary>
    /// Gets tags for a specific task asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of tags for the task.</returns>
    public async Task<IEnumerable<TagWebApiModel>> GetTagsForTaskAsync(int taskId)
    {
        var response = await this.httpClient.GetFromJsonAsync<IEnumerable<TagWebApiModel>>($"api/taskItem/{taskId}/tags");
        ArgumentNullException.ThrowIfNull(response);
        return response;
    }
}
