using System.Net.Http.Json;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.Services.Implementations;
public class TodoListWebApiService : ITodoListWebApiService
{
    private readonly HttpClient httpClient;

    public TodoListWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<TodoListWebApiModel>> GetTodoListsAsync()
    {
        var response = await this.httpClient.GetFromJsonAsync<IEnumerable<TodoListWebApiModel>>("api/TodoList");
        return response ?? new List<TodoListWebApiModel>();
    }

    public async Task<TodoListWebApiModel> GetTodoListAsync(int id)
    {
        var response = await this.httpClient.GetFromJsonAsync<TodoListWebApiModel>($"api/TodoList/{id}");
        ArgumentNullException.ThrowIfNull(response);

        return response;
    }

    public async Task<TodoListWebApiModel> CreateTodoListAsync(TodoListWebApiModel todoList)
    {
        var response = await this.httpClient.PostAsJsonAsync("api/TodoList", todoList);
        response.EnsureSuccessStatusCode();
        var createdTodoList = await response.Content.ReadFromJsonAsync<TodoListWebApiModel>();
        ArgumentNullException.ThrowIfNull(createdTodoList);
        return createdTodoList;
    }

    public async Task DeleteTodoListAsync(int id)
    {
        var response = await this.httpClient.DeleteAsync($"api/TodoList/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<TodoListWebApiModel> UpdateTodoListAsync(int id, TodoListWebApiModel todoList)
    {
        var response = await this.httpClient.PutAsJsonAsync($"api/TodoList/{id}", todoList);
        response.EnsureSuccessStatusCode();
        var updatedTodoList = await response.Content.ReadFromJsonAsync<TodoListWebApiModel>();
        ArgumentNullException.ThrowIfNull(updatedTodoList);
        return updatedTodoList;
    }
}
