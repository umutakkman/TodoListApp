using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Implementations;

/// <summary>
/// Service for managing comments in the database.
/// </summary>
public class CommentDatabaseService : ICommentDatabaseService
{
    private readonly ApplicationDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentDatabaseService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public CommentDatabaseService(ApplicationDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets the collection of comments.
    /// </summary>
    public IQueryable<CommentEntity> Comments => this.context.Comment;

    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="comment">The comment entity to create.</param>
    /// <exception cref="ArgumentNullException">Thrown when the comment is null.</exception>
    public void CreateComment(CommentEntity comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        _ = this.context.Add(comment);
        _ = this.context.SaveChanges();
    }

    /// <summary>
    /// Updates an existing comment.
    /// </summary>
    /// <param name="comment">The comment entity to update.</param>
    /// <exception cref="ArgumentNullException">Thrown when the comment is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the comment is not found.</exception>
    public void UpdateComment(CommentEntity comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        var existing = this.context.Comment.FirstOrDefault(x => x.Id == comment.Id);
        if (existing == null)
        {
            throw new InvalidOperationException("Comment not found");
        }

        this.context.Entry(existing).CurrentValues.SetValues(comment);
        _ = this.context.SaveChanges();
    }

    /// <summary>
    /// Deletes a comment.
    /// </summary>
    /// <param name="comment">The comment entity to delete.</param>
    /// <exception cref="ArgumentNullException">Thrown when the comment is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the comment is not found.</exception>
    public void DeleteComment(CommentEntity comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        var existing = this.context.Comment.FirstOrDefault(x => x.Id == comment.Id);
        if (existing == null)
        {
            throw new InvalidOperationException("Comment not found");
        }

        _ = this.context.Remove(comment);
        _ = this.context.SaveChanges();
    }
}
