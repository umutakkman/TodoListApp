using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.Services.Interfaces;

/// <summary>
/// Interface for comment Web API service.
/// </summary>
public interface ICommentWebApiService
{
    /// <summary>
    /// Gets a comment asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="commentId">The ID of the comment.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the comment.</returns>
    Task<CommentWebApiModel> GetCommentAsync(int taskId, int commentId);

    /// <summary>
    /// Creates a new comment asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="comment">The comment to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created comment.</returns>
    Task<CommentWebApiModel> CreateCommentAsync(int taskId, CommentWebApiModel comment);

    /// <summary>
    /// Deletes a comment asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="commentId">The ID of the comment.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteCommentAsync(int taskId, int commentId);

    /// <summary>
    /// Updates a comment asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="commentId">The ID of the comment.</param>
    /// <param name="comment">The comment to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated comment.</returns>
    Task<CommentWebApiModel> UpdateCommentAsync(int taskId, int commentId, CommentWebApiModel comment);
}
