using System.Net.Http.Json;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.Services.Implementations;
public class TagWebApiService : ITagWebApiService
{
    private readonly HttpClient httpClient;

    public TagWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<TagWebApiModel>> GetAllTagsAsync()
    {
        var response = await this.httpClient.GetFromJsonAsync<IEnumerable<TagWebApiModel>>("api/Tag");
        ArgumentNullException.ThrowIfNull(response);
        return response;
    }

    public async Task<IEnumerable<TagWebApiModel>> GetTagsForTaskAsync(int taskId)
    {
        var response = await this.httpClient.GetFromJsonAsync<IEnumerable<TagWebApiModel>>($"api/taskItem/{taskId}/tags");
        ArgumentNullException.ThrowIfNull(response);
        return response;
    }

    public async Task<IEnumerable<TaskItemWebApiModel>> GetTasksByTagAsync(int tagId)
    {
        var response = await this.httpClient.GetFromJsonAsync<IEnumerable<TaskItemWebApiModel>>($"api/taskItem/bytag/{tagId}");
        ArgumentNullException.ThrowIfNull(response);
        return response;
    }

    public async Task<IEnumerable<TagWebApiModel>> AddTagToTaskAsync(int taskId, int tagId)
    {
        var response = await this.httpClient.PostAsJsonAsync($"api/taskItem/{taskId}/tags", tagId);
        _ = response.EnsureSuccessStatusCode();
        var updatedTags = await response.Content.ReadFromJsonAsync<IEnumerable<TagWebApiModel>>();
        ArgumentNullException.ThrowIfNull(updatedTags);
        return updatedTags;
    }

    public async Task RemoveTagFromTaskAsync(int taskId, int tagId)
    {
        var uri = new Uri($"api/taskItem/{taskId}/tag/{tagId}", UriKind.Relative);
        var response = await this.httpClient.DeleteAsync(uri);
        _ = response.EnsureSuccessStatusCode();
    }
}
