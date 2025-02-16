using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.Services.Interfaces;

/// <summary>
/// Interface for to-do list Web API service.
/// </summary>
public interface ITodoListWebApiService
{
    /// <summary>
    /// Gets all to-do lists asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of to-do lists.</returns>
    Task<IEnumerable<TodoListWebApiModel>> GetTodoListsAsync();

    /// <summary>
    /// Gets a to-do list asynchronously.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the to-do list.</returns>
    Task<TodoListWebApiModel> GetTodoListAsync(int id);

    /// <summary>
    /// Creates a new to-do list asynchronously.
    /// </summary>
    /// <param name="todoList">The to-do list to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created to-do list.</returns>
    Task<TodoListWebApiModel> CreateTodoListAsync(TodoListWebApiModel todoList);

    /// <summary>
    /// Deletes a to-do list asynchronously.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteTodoListAsync(int id);

    /// <summary>
    /// Updates a to-do list asynchronously.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <param name="todoList">The to-do list to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated to-do list.</returns>
    Task<TodoListWebApiModel> UpdateTodoListAsync(int id, TodoListWebApiModel todoList);
}
