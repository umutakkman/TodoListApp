using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.WebApp.Controllers;
public class TodoListController : Controller
{
    private readonly ITodoListWebApiService todoListWebApiService;
    private readonly UserManager<IdentityUser> userManager;

    public TodoListController(ITodoListWebApiService todoListWebApiService, UserManager<IdentityUser> userManager)
    {
        this.todoListWebApiService = todoListWebApiService;
        this.userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var todoLists = await this.todoListWebApiService.GetTodoListsAsync();
        todoLists = todoLists.Where(todoList => todoList.OwnerId == this.User.FindFirstValue(ClaimTypes.NameIdentifier));
        return this.View(todoLists);
    }

    public async Task<IActionResult> Details(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var todoList = await this.todoListWebApiService.GetTodoListAsync(id);

        if (todoList == null)
        {
            return this.NotFound();
        }

        if (todoList.OwnerId != this.userManager.GetUserId(this.User))
        {
            return this.Forbid();
        }

        return this.View(todoList);
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

        var ownerId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(ownerId))
        {
            return this.BadRequest("Invalid owner ID.");
        }

        ArgumentNullException.ThrowIfNull(todoList);

        todoList.OwnerId = ownerId;

        var createdTodoList = await this.todoListWebApiService.CreateTodoListAsync(todoList);
        return this.RedirectToAction(nameof(this.Details), new { id = createdTodoList.Id });
    }

    public async Task<IActionResult> Edit(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var todoList = await this.todoListWebApiService.GetTodoListAsync(id);
        if (todoList == null)
        {
            return this.NotFound();
        }

        if (todoList.OwnerId != this.userManager.GetUserId(this.User))
        {
            return this.Forbid();
        }

        return this.View(todoList);
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

        var todoList = await this.todoListWebApiService.GetTodoListAsync(id);
        if (todoList == null)
        {
            return this.NotFound();
        }

        if (todoList.OwnerId != this.userManager.GetUserId(this.User))
        {
            return this.Forbid();
        }

        return this.View(todoList);
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
}
