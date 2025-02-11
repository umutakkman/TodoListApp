using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Models;

namespace TodoListApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoListController : ControllerBase
{
    private readonly ITodoListDatabaseService todoListDatabaseService;

    public TodoListController(ITodoListDatabaseService todoListDatabaseService)
    {
        this.todoListDatabaseService = todoListDatabaseService;
    }

    [HttpGet]
    public ActionResult<TodoList> GetTodoLists()
    {
        var lists = this.todoListDatabaseService.TodoLists.ToList();
        return this.Ok(lists);
    }

    [HttpGet("{id:int}")]
    public ActionResult<TodoList> GetTodoListById(int id)
    {
        var list = this.todoListDatabaseService.TodoLists.FirstOrDefault(x => x.Id == id);
        if (list == null)
        {
            return this.NotFound();
        }

        return this.Ok(list);
    }

    [HttpPost]
    public ActionResult<TodoList> CreateTodoList([FromBody] TodoList todoList)
    {
        if (todoList == null)
        {
            return this.BadRequest("Invalid todo list data.");
        }

        this.todoListDatabaseService.CreateTodoList(todoList);

        return this.CreatedAtAction(nameof(this.GetTodoListById), new { id = todoList.Id }, todoList);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateTodoList(int id, [FromBody] TodoList todoList)
    {
        ArgumentNullException.ThrowIfNull(todoList);

        if (id != todoList.Id)
        {
            return this.BadRequest("ID mismatch.");
        }

        var existing = this.todoListDatabaseService.TodoLists.FirstOrDefault(x => x.Id == id);
        if (existing == null)
        {
            return this.NotFound();
        }

        this.todoListDatabaseService.UpdateTodoList(todoList);
        return this.NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteTodoList(int id)
    {
        var todoList = this.todoListDatabaseService.TodoLists.FirstOrDefault(x => x.Id == id);
        if (todoList == null)
        {
            return this.NotFound();
        }

        this.todoListDatabaseService.DeleteTodoList(todoList);
        return this.NoContent();
    }
}
