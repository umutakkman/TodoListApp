using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Interfaces;

/// <summary>
/// Interface for comment database service.
/// </summary>
public interface ICommentDatabaseService
{
    /// <summary>
    /// Gets the collection of comments.
    /// </summary>
    IQueryable<CommentEntity> Comments { get; }

    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="comment">The comment entity to create.</param>
    void CreateComment(CommentEntity comment);

    /// <summary>
    /// Updates an existing comment.
    /// </summary>
    /// <param name="comment">The comment entity to update.</param>
    void UpdateComment(CommentEntity comment);

    /// <summary>
    /// Deletes a comment.
    /// </summary>
    /// <param name="comment">The comment entity to delete.</param>
    void DeleteComment(CommentEntity comment);
}
