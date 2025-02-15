using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Interfaces;
public interface ICommentDatabaseService
{
    IQueryable<CommentEntity> Comments { get; }

    void CreateComment(CommentEntity comment);

    void UpdateComment(CommentEntity comment);

    void DeleteComment(CommentEntity comment);
}
