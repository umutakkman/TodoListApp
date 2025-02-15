using System.Net.Http.Json;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.Services.Implementations;
public class CommentWebApiService : ICommentWebApiService
{
    private readonly HttpClient httpClient;

    public CommentWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<CommentWebApiModel> GetCommentAsync(int taskId, int commentId)
    {
        var response = await this.httpClient.GetFromJsonAsync<CommentWebApiModel>($"api/TaskItem/{taskId}/comments/{commentId}");
        ArgumentNullException.ThrowIfNull(response);
        return response;
    }

    public async Task<CommentWebApiModel> CreateCommentAsync(int taskId, CommentWebApiModel comment)
    {
        var response = await this.httpClient.PostAsJsonAsync($"api/TaskItem/{taskId}/comments", comment);
        _ = response.EnsureSuccessStatusCode();
        var createdComment = await response.Content.ReadFromJsonAsync<CommentWebApiModel>();
        ArgumentNullException.ThrowIfNull(createdComment);
        return createdComment;
    }

    public async Task<CommentWebApiModel> UpdateCommentAsync(int taskId, int commentId, CommentWebApiModel comment)
    {
        var response = await this.httpClient.PutAsJsonAsync($"api/TaskItem/{taskId}/comments/{commentId}", comment);
        _ = response.EnsureSuccessStatusCode();
        var updatedComment = await response.Content.ReadFromJsonAsync<CommentWebApiModel>();
        ArgumentNullException.ThrowIfNull(updatedComment);
        return updatedComment;
    }

    public async Task DeleteCommentAsync(int taskId, int commentId)
    {
        var uri = new Uri($"api/TaskItem/{taskId}/comments/{commentId}", UriKind.Relative);
        var response = await this.httpClient.DeleteAsync(uri);
        _ = response.EnsureSuccessStatusCode();
    }
}
