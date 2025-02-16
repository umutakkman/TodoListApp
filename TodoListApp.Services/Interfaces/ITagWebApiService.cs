using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.Services.Interfaces;

/// <summary>
/// Interface for tag Web API service.
/// </summary>
public interface ITagWebApiService
{
    /// <summary>
    /// Gets all tags asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of tags.</returns>
    Task<IEnumerable<TagWebApiModel>> GetAllTagsAsync();

    /// <summary>
    /// Gets tags for a specific task asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of tags for the task.</returns>
    Task<IEnumerable<TagWebApiModel>> GetTagsForTaskAsync(int taskId);

    /// <summary>
    /// Gets tasks by a specific tag asynchronously.
    /// </summary>
    /// <param name="tagId">The ID of the tag.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of tasks associated with the tag.</returns>
    Task<IEnumerable<TaskItemWebApiModel>> GetTasksByTagAsync(int tagId);

    /// <summary>
    /// Adds a tag to a task asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="tagId">The ID of the tag.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated collection of tags for the task.</returns>
    Task<IEnumerable<TagWebApiModel>> AddTagToTaskAsync(int taskId, int tagId);

    /// <summary>
    /// Removes a tag from a task asynchronously.
    /// </summary>
    /// <param name="taskId">The ID of the task.</param>
    /// <param name="tagId">The ID of the tag.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveTagFromTaskAsync(int taskId, int tagId);
}
