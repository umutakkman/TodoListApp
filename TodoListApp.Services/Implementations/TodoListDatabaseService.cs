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
        ArgumentNullException.ThrowIfNull(todoList);

        this.context.Add(todoList);
        this.context.SaveChanges();
    }

    public void UpdateTodoList(TodoList todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);

        var existing = this.context.TodoLists.FirstOrDefault(x => x.Id == todoList.Id);
        if (existing == null)
        {
            throw new InvalidOperationException("Task item not found");
        }

        this.context.Update(todoList);
        this.context.SaveChanges();
    }

    public void DeleteTodoList(TodoList todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);

        var existing = this.context.TodoLists.FirstOrDefault(x => x.Id == todoList.Id);
        if (existing == null)
        {
            throw new InvalidOperationException("Task item not found");
        }

        this.context.Remove(todoList);
        this.context.SaveChanges();
    }
}
