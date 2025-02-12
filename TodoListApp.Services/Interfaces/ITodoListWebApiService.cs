using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.Services.Interfaces;
public interface ITodoListWebApiService
{
    Task<IEnumerable<TodoListWebApiModel>> GetTodoListsAsync();

    Task<TodoListWebApiModel> GetTodoListAsync(int id);

    Task<TodoListWebApiModel> CreateTodoListAsync(TodoListWebApiModel todoList);

    Task DeleteTodoListAsync(int id);

    Task<TodoListWebApiModel> UpdateTodoListAsync(int id, TodoListWebApiModel todoList);
}
