using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Implementations;
public class TodoListDatabaseService : ITodoListDatabaseService
{
    private readonly TodoListDbContext context;

    public TodoListDatabaseService(TodoListDbContext context)
    {
        this.context = context;
    }

    public IQueryable<TodoListEntity> TodoLists => this.context.TodoList;

    public void CreateTodoList(TodoListEntity todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);

        _ = this.context.Add(todoList);
        _ = this.context.SaveChanges();
    }

    public void UpdateTodoList(TodoListEntity todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);

        var existing = this.context.TodoList.FirstOrDefault(x => x.Id == todoList.Id);
        if (existing == null)
        {
            throw new InvalidOperationException("Task item not found");
        }

        this.context.Entry(existing).CurrentValues.SetValues(todoList);
        _ = this.context.SaveChanges();
    }

    public void DeleteTodoList(TodoListEntity todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);

        var existing = this.context.TodoList.FirstOrDefault(x => x.Id == todoList.Id);
        if (existing == null)
        {
            throw new InvalidOperationException("Task item not found");
        }

        _ = this.context.Remove(todoList);
        _ = this.context.SaveChanges();
    }
}
