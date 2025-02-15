using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.Services.Interfaces;
public interface ICommentWebApiService
{
    Task<CommentWebApiModel> GetCommentAsync(int taskId, int commentId);

    Task<CommentWebApiModel> CreateCommentAsync(int taskId, CommentWebApiModel comment);

    Task DeleteCommentAsync(int taskId, int commentId);

    Task<CommentWebApiModel> UpdateCommentAsync(int taskId, int commentId, CommentWebApiModel comment);
}
