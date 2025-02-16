using System.Net.Http.Json;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.Services.Implementations;

/// <summary>
/// Service for managing tags via Web API.
/// </summary>
public class TagWebApiService : ITagWebApiService
{
    private readonly HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="TagWebApiService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public TagWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /// <summary>
    /// Gets all tags asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of tags.</returns>
    public async Task<IEnumerable<TagWebApiModel>> GetAllTagsAsync()
    {
        var response = await this.httpClient.GetFromJsonAsync<IEnumerable<TagWebApiModel>>("api/Tag");
        ArgumentNullException.ThrowIfNull(response);
        return response;
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

    /// <summary>
    /// Gets tasks by a specific tag asynchronously.
    /// </summary>
    /// <param name="tagId">The ID of the tag.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of tasks associated with the tag.</returns>
    public async Task<IEnumerable<TaskItemWebApiModel>> GetTasksByTagAsync(int tagId)
    {
        var response = await this.httpClient.GetFromJsonAsync<IEnumerable<TaskItemWebApiModel>>($"api/taskItem/bytag/{tagId}");
        ArgumentNullException.ThrowIfNull(response);
        return response;
    }

    /// <summary>
    /// Adds a tag to a task asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="tagId">The ID of the tag.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated collection of tags for the task.</returns>
    public async Task<IEnumerable<TagWebApiModel>> AddTagToTaskAsync(int taskId, int tagId)
    {
        var response = await this.httpClient.PostAsJsonAsync($"api/taskItem/{taskId}/tags", tagId);
        _ = response.EnsureSuccessStatusCode();
        var updatedTags = await response.Content.ReadFromJsonAsync<IEnumerable<TagWebApiModel>>();
        ArgumentNullException.ThrowIfNull(updatedTags);
        return updatedTags;
    }

    /// <summary>
    /// Removes a tag from a task asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="tagId">The ID of the tag.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RemoveTagFromTaskAsync(int taskId, int tagId)
    {
        var uri = new Uri($"api/taskItem/{taskId}/tag/{tagId}", UriKind.Relative);
        var response = await this.httpClient.DeleteAsync(uri);
        _ = response.EnsureSuccessStatusCode();
    }
}
