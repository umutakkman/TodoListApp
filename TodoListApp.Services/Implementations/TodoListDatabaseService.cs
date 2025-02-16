using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Implementations;

/// <summary>
/// Service for managing to-do lists in the database.
/// </summary>
public class TodoListDatabaseService : ITodoListDatabaseService
{
    private readonly ApplicationDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListDatabaseService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public TodoListDatabaseService(ApplicationDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets the collection of to-do lists.
    /// </summary>
    public IQueryable<TodoListEntity> TodoLists => this.context.TodoList;

    /// <summary>
    /// Creates a new to-do list.
    /// </summary>
    /// <param name="todoList">The to-do list entity to create.</param>
    /// <exception cref="ArgumentNullException">Thrown when the to-do list is null.</exception>
    public void CreateTodoList(TodoListEntity todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);

        _ = this.context.Add(todoList);
        _ = this.context.SaveChanges();
    }

    /// <summary>
    /// Updates an existing to-do list.
    /// </summary>
    /// <param name="todoList">The to-do list entity to update.</param>
    /// <exception cref="ArgumentNullException">Thrown when the to-do list is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the to-do list is not found.</exception>
    public void UpdateTodoList(TodoListEntity todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);

        var existing = this.context.TodoList.FirstOrDefault(x => x.Id == todoList.Id);
        if (existing == null)
        {
            throw new InvalidOperationException("To-do list not found");
        }

        this.context.Entry(existing).CurrentValues.SetValues(todoList);
        _ = this.context.SaveChanges();
    }

    /// <summary>
    /// Deletes a to-do list.
    /// </summary>
    /// <param name="todoList">The to-do list entity to delete.</param>
    /// <exception cref="ArgumentNullException">Thrown when the to-do list is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the to-do list is not found.</exception>
    public void DeleteTodoList(TodoListEntity todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);

        var existing = this.context.TodoList.FirstOrDefault(x => x.Id == todoList.Id);
        if (existing == null)
        {
            throw new InvalidOperationException("To-do list not found");
        }

        _ = this.context.Remove(todoList);
        _ = this.context.SaveChanges();
    }
}
