using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.Services.Interfaces;
public interface ITagWebApiService
{
    Task<IEnumerable<TagWebApiModel>> GetAllTagsAsync();

    Task<IEnumerable<TagWebApiModel>> GetTagsForTaskAsync(int taskId);

    Task<IEnumerable<TaskItemWebApiModel>> GetTasksByTagAsync(int tagId);

    Task<IEnumerable<TagWebApiModel>> AddTagToTaskAsync(int taskId, TagWebApiModel tag);

    Task RemoveTagFromTaskAsync(int taskId, int tagId);
}
