using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Interfaces;

/// <summary>
/// Interface for to-do list database service.
/// </summary>
public interface ITodoListDatabaseService
{
    /// <summary>
    /// Gets the collection of to-do lists.
    /// </summary>
    IQueryable<TodoListEntity> TodoLists { get; }

    /// <summary>
    /// Creates a new to-do list.
    /// </summary>
    /// <param name="todoList">The to-do list entity to create.</param>
    void CreateTodoList(TodoListEntity todoList);

    /// <summary>
    /// Updates an existing to-do list.
    /// </summary>
    /// <param name="todoList">The to-do list entity to update.</param>
    void UpdateTodoList(TodoListEntity todoList);

    /// <summary>
    /// Deletes a to-do list.
    /// </summary>
    /// <param name="todoList">The to-do list entity to delete.</param>
    void DeleteTodoList(TodoListEntity todoList);
}
