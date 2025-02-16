using System.Net.Http.Json;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.Services.Implementations;

/// <summary>
/// Service for managing to-do lists via Web API.
/// </summary>
public class TodoListWebApiService : ITodoListWebApiService
{
    private readonly HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListWebApiService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public TodoListWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /// <summary>
    /// Gets all to-do lists asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of to-do lists.</returns>
    public async Task<IEnumerable<TodoListWebApiModel>> GetTodoListsAsync()
    {
        var response = await this.httpClient.GetFromJsonAsync<IEnumerable<TodoListWebApiModel>>("api/TodoList");
        return response ?? new List<TodoListWebApiModel>();
    }

    /// <summary>
    /// Gets a to-do list asynchronously.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the to-do list.</returns>
    public async Task<TodoListWebApiModel> GetTodoListAsync(int id)
    {
        var response = await this.httpClient.GetFromJsonAsync<TodoListWebApiModel>($"api/TodoList/{id}");
        ArgumentNullException.ThrowIfNull(response);

        return response;
    }

    /// <summary>
    /// Creates a new to-do list asynchronously.
    /// </summary>
    /// <param name="todoList">The to-do list to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created to-do list.</returns>
    public async Task<TodoListWebApiModel> CreateTodoListAsync(TodoListWebApiModel todoList)
    {
        var response = await this.httpClient.PostAsJsonAsync("api/TodoList", todoList);
        _ = response.EnsureSuccessStatusCode();
        var createdTodoList = await response.Content.ReadFromJsonAsync<TodoListWebApiModel>();
        ArgumentNullException.ThrowIfNull(createdTodoList);
        return createdTodoList;
    }

    /// <summary>
    /// Deletes a to-do list asynchronously.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteTodoListAsync(int id)
    {
        var uri = new Uri($"api/TodoList/{id}", UriKind.Relative);
        var response = await this.httpClient.DeleteAsync(uri);
        _ = response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Updates a to-do list asynchronously.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <param name="todoList">The to-do list to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated to-do list.</returns>
    public async Task<TodoListWebApiModel> UpdateTodoListAsync(int id, TodoListWebApiModel todoList)
    {
        var response = await this.httpClient.PutAsJsonAsync($"api/TodoList/{id}", todoList);
        _ = response.EnsureSuccessStatusCode();
        var updatedTodoList = await response.Content.ReadFromJsonAsync<TodoListWebApiModel>();
        ArgumentNullException.ThrowIfNull(updatedTodoList);
        return updatedTodoList;
    }
}
