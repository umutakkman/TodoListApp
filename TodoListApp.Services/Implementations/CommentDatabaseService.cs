using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Implementations;
public class CommentDatabaseService : ICommentDatabaseService
{
    private readonly ApplicationDbContext context;

    public CommentDatabaseService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IQueryable<CommentEntity> Comments => this.context.Comment;

    public void CreateComment(CommentEntity comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        _ = this.context.Add(comment);
        _ = this.context.SaveChanges();
    }

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
