using TodoListApp.WebApi.Models.Models;

namespace TodoListApp.Services.Interfaces;

public interface ITodoListDatabaseService
{
    IQueryable<TodoList> TodoLists { get; }

    void CreateTodoList(TodoList todoList);

    void UpdateTodoList(TodoList todoList);

    void DeleteTodoList(TodoList todoList);
}
