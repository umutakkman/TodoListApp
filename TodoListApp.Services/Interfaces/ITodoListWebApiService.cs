using TodoListApp.WebApi.Models;

namespace TodoListApp.Services.Interfaces;
public interface ITodoListWebApiService
{
    Task<IEnumerable<TodoListWebApiModel>> GetTodoListsAsync();
}
