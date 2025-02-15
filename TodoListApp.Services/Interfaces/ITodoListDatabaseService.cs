using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Interfaces;

public interface ITodoListDatabaseService
{
    IQueryable<TodoListEntity> TodoLists { get; }

    void CreateTodoList(TodoListEntity todoList);

    void UpdateTodoList(TodoListEntity todoList);

    void DeleteTodoList(TodoListEntity todoList);
}
