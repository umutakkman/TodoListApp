using System.Net.Http.Json;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models;

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
}
