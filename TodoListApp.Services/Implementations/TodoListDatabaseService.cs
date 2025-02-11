using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Models;

namespace TodoListApp.Services.Implementations;
public class TodoListDatabaseService : ITodoListDatabaseService
{
    private readonly TodoListDbContext context;

    public TodoListDatabaseService(TodoListDbContext context)
    {
        this.context = context;
    }

    public IQueryable<TodoList> TodoLists => this.context.TodoLists;

    public void CreateTodoList(TodoList todoList)
    {
        this.context.Add(todoList);
        this.context.SaveChanges();
    }

    public void UpdateTodoList(TodoList todoList)
    {
        this.context.Update(todoList);
        this.context.SaveChanges();
    }

    public void DeleteTodoList(TodoList todoList)
    {
        this.context.Remove(todoList);
        this.context.SaveChanges();
    }
}
