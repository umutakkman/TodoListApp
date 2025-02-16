using System.Net.Http.Json;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.Services.Implementations;

/// <summary>
/// Service for managing comments via Web API.
/// </summary>
public class CommentWebApiService : ICommentWebApiService
{
    private readonly HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentWebApiService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public CommentWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /// <summary>
    /// Gets a comment asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="commentId">The ID of the comment.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the comment.</returns>
    public async Task<CommentWebApiModel> GetCommentAsync(int taskId, int commentId)
    {
        var response = await this.httpClient.GetFromJsonAsync<CommentWebApiModel>($"api/TaskItem/{taskId}/comments/{commentId}");
        ArgumentNullException.ThrowIfNull(response);
        return response;
    }

    /// <summary>
    /// Creates a new comment asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="comment">The comment to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created comment.</returns>
    public async Task<CommentWebApiModel> CreateCommentAsync(int taskId, CommentWebApiModel comment)
    {
        var response = await this.httpClient.PostAsJsonAsync($"api/TaskItem/{taskId}/comments", comment);
        _ = response.EnsureSuccessStatusCode();
        var createdComment = await response.Content.ReadFromJsonAsync<CommentWebApiModel>();
        ArgumentNullException.ThrowIfNull(createdComment);
        return createdComment;
    }

    /// <summary>
    /// Updates a comment asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="commentId">The ID of the comment.</param>
    /// <param name="comment">The comment to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated comment.</returns>
    public async Task<CommentWebApiModel> UpdateCommentAsync(int taskId, int commentId, CommentWebApiModel comment)
    {
        var response = await this.httpClient.PutAsJsonAsync($"api/TaskItem/{taskId}/comments/{commentId}", comment);
        _ = response.EnsureSuccessStatusCode();
        var updatedComment = await response.Content.ReadFromJsonAsync<CommentWebApiModel>();
        ArgumentNullException.ThrowIfNull(updatedComment);
        return updatedComment;
    }

    /// <summary>
    /// Deletes a comment asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="commentId">The ID of the comment.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteCommentAsync(int taskId, int commentId)
    {
        var uri = new Uri($"api/TaskItem/{taskId}/comments/{commentId}", UriKind.Relative);
        var response = await this.httpClient.DeleteAsync(uri);
        _ = response.EnsureSuccessStatusCode();
    }
}
