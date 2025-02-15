using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.WebApp.Controllers;
public class TodoListController : Controller
{
    private readonly ITodoListWebApiService todoListWebApiService;

    public TodoListController(ITodoListWebApiService todoListWebApiService)
    {
        this.todoListWebApiService = todoListWebApiService;
    }

    public async Task<IActionResult> Index()
    {
        var todoLists = await this.todoListWebApiService.GetTodoListsAsync();
        return this.View(todoLists);
    }

    public async Task<IActionResult> Details(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        return await this.GetTodoListViewById(id);
    }

    public IActionResult Create()
    {
        return this.View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoListWebApiModel todoList)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var createdTodoList = await this.todoListWebApiService.CreateTodoListAsync(todoList);
        return this.RedirectToAction(nameof(this.Details), new { id = createdTodoList.Id });
    }

    public async Task<IActionResult> Edit(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        return await this.GetTodoListViewById(id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TodoListWebApiModel todoList)
    {
        if (todoList == null)
        {
            return this.BadRequest("Invalid todo list data.");
        }

        if (id != todoList.Id)
        {
            return this.BadRequest("ID mismatch.");
        }

        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var updatedTodoList = await this.todoListWebApiService.UpdateTodoListAsync(id, todoList);
        return this.RedirectToAction(nameof(this.Details), new { id = updatedTodoList.Id });
    }

    public async Task<IActionResult> Delete(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        return await this.GetTodoListViewById(id);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        await this.todoListWebApiService.DeleteTodoListAsync(id);
        return this.RedirectToAction(nameof(this.Index));
    }

    private async Task<IActionResult> GetTodoListViewById(int id)
    {
        var todoList = await this.todoListWebApiService.GetTodoListAsync(id);
        if (todoList == null)
        {
            return this.NotFound();
        }

        return this.View(todoList);
    }
}
