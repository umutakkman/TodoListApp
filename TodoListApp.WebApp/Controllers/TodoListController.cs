using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;
using TaskStatus = TodoListApp.Common.TaskStatus;

namespace TodoListApp.WebApp.Controllers;

/// <summary>
/// Controller for managing to-do lists.
/// </summary>
public class TodoListController : Controller
{
    private readonly ITodoListWebApiService todoListWebApiService;
    private readonly UserManager<IdentityUser> userManager;
    private readonly ITaskItemWebApiService taskItemWebApiService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListController"/> class.
    /// </summary>
    /// <param name="todoListWebApiService">The to-do list Web API service.</param>
    /// <param name="userManager">The user manager.</param>
    public TodoListController(ITodoListWebApiService todoListWebApiService, UserManager<IdentityUser> userManager, ITaskItemWebApiService taskItemWebApiService)
    {
        this.todoListWebApiService = todoListWebApiService;
        this.userManager = userManager;
        this.taskItemWebApiService = taskItemWebApiService;
    }

    /// <summary>
    /// Displays the list of to-do lists.
    /// </summary>
    /// <returns>The view with the list of to-do lists.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var todoLists = await this.todoListWebApiService.GetTodoListsAsync();
        todoLists = todoLists.Where(todoList => todoList.OwnerId == this.User.FindFirstValue(ClaimTypes.NameIdentifier));
        return this.View(todoLists);
    }

    /// <summary>
    /// Displays the details of a to-do list.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <returns>The to-do list details view.</returns>
    [HttpGet]
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

        this.ViewData["IsDetails"] = true;

        return this.View(todoList);
    }

    /// <summary>
    /// Displays the create to-do list page.
    /// </summary>
    /// <returns>The create to-do list view.</returns>
    [HttpGet]
    public IActionResult Create()
    {
        return this.View();
    }

    /// <summary>
    /// Handles the create to-do list form submission.
    /// </summary>
    /// <param name="todoList">The to-do list view model.</param>
    /// <returns>The result of the create to-do list process.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoListWebApiModel todoList)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var validationResult = this.ValidateCreateParameters(todoList);
        if (validationResult != null)
        {
            return validationResult;
        }

        return await this.HandleCreateAsync(todoList);
    }

    /// <summary>
    /// Displays the edit to-do list page.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <returns>The edit to-do list view.</returns>
    [HttpGet]
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

        this.ViewData["IsEdit"] = true;

        return this.View(todoList);
    }

    /// <summary>
    /// Handles the edit to-do list form submission.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <param name="todoList">The to-do list view model.</param>
    /// <returns>The result of the edit to-do list process.</returns>
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

    /// <summary>
    /// Displays the delete to-do list page.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <returns>The delete to-do list view.</returns>
    [HttpGet]
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

        this.ViewData["IsDelete"] = true;

        return this.View(todoList);
    }

    /// <summary>
    /// Handles the delete to-do list form submission.
    /// </summary>
    /// <param name="id">The ID of the to-do list.</param>
    /// <returns>The result of the delete to-do list process.</returns>
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

    /// <summary>
    /// Updates the status of a task and redirects back to the TodoList details.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="status">The new status of the task item.</param>
    /// <param name="todoListId">The ID of the to-do list to redirect back to.</param>
    /// <returns>The result of the update task status process.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, TaskStatus status, int todoListId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var updatedTask = await this.taskItemWebApiService.UpdateTaskStatusAsync(id, status);
        if (updatedTask == null)
        {
            return this.NotFound();
        }

        // Redirect back to the TodoList details page
        return this.RedirectToAction("Details", "TodoList", new { id = todoListId });
    }

    /// <summary>
    /// Validates the parameters for creating a to-do list.
    /// </summary>
    /// <param name="todoList">The to-do list view model.</param>
    /// <returns>A <see cref="BadRequestObjectResult"/> if validation fails; otherwise, null.</returns>
    private BadRequestObjectResult? ValidateCreateParameters(TodoListWebApiModel todoList)
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

        return null;
    }

    /// <summary>
    /// Handles the create to-do list process asynchronously.
    /// </summary>
    /// <param name="todoList">The to-do list view model.</param>
    /// <returns>The result of the create to-do list process.</returns>
    private async Task<IActionResult> HandleCreateAsync(TodoListWebApiModel todoList)
    {
        var createdTodoList = await this.todoListWebApiService.CreateTodoListAsync(todoList);
        return this.RedirectToAction(nameof(this.Details), new { id = createdTodoList.Id });
    }
}
