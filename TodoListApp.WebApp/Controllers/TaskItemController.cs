using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;
using TodoListApp.WebApi.Models.ViewModels;
using TaskStatus = TodoListApp.Common.TaskStatus;

namespace TodoListApp.WebApp.Controllers;

/// <summary>
/// Controller for managing task items.
/// </summary>
public class TaskItemController : Controller
{
    private readonly ITaskItemWebApiService taskItemWebApiService;
    private readonly ITagWebApiService tagWebApiService;
    private readonly ICommentWebApiService commentWebApiService;
    private readonly ITodoListWebApiService todoListWebApiService;
    private readonly UserManager<IdentityUser> userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskItemController"/> class.
    /// </summary>
    /// <param name="taskItemWebApiService">The task item Web API service.</param>
    /// <param name="tagWebApiService">The tag Web API service.</param>
    /// <param name="commentWebApiService">The comment Web API service.</param>
    /// <param name="todoListWebApiService">The to-do list Web API service.</param>
    /// <param name="userManager">The user manager.</param>
    public TaskItemController(ITaskItemWebApiService taskItemWebApiService, ITagWebApiService tagWebApiService, ICommentWebApiService commentWebApiService, ITodoListWebApiService todoListWebApiService, UserManager<IdentityUser> userManager)
    {
        this.taskItemWebApiService = taskItemWebApiService;
        this.tagWebApiService = tagWebApiService;
        this.commentWebApiService = commentWebApiService;
        this.todoListWebApiService = todoListWebApiService;
        this.userManager = userManager;
    }

    /// <summary>
    /// Displays the details of a task item.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <returns>The task item details view.</returns>
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var taskItem = await this.taskItemWebApiService.GetTaskItemAsync(id);

        if (taskItem == null)
        {
            return this.NotFound();
        }

        var todoList = await this.todoListWebApiService.GetTodoListAsync(taskItem.TodoListId);

        if (todoList == null)
        {
            return this.NotFound();
        }

        var currentUserId = this.userManager.GetUserId(this.User);

        if (taskItem.UserId != this.userManager.GetUserId(this.User) && todoList.OwnerId != currentUserId)
        {
            return this.Forbid();
        }

        this.ViewBag.IsOwner = todoList.OwnerId == currentUserId;

        return this.View(taskItem);
    }

    /// <summary>
    /// Displays the create task item page.
    /// </summary>
    /// <param name="todoListId">The ID of the to-do list.</param>
    /// <returns>The create task item view.</returns>
    [HttpGet]
    public async Task<IActionResult> Create(int todoListId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var todoList = await this.todoListWebApiService.GetTodoListAsync(todoListId);
        if (todoList == null)
        {
            return this.NotFound();
        }

        if (todoList.OwnerId != this.userManager.GetUserId(this.User))
        {
            return this.Forbid();
        }

        var users = this.userManager.Users.ToList();
        var userSelectList = users.Select(u => new SelectListItem
        {
            Value = u.Id,
            Text = u.Email,
        }).ToList();

        this.ViewBag.UserList = userSelectList;

        var model = new TaskItemWebApiModel
        {
            TodoListId = todoListId,
        };

        return this.View(model);
    }

    /// <summary>
    /// Handles the create task item form submission.
    /// </summary>
    /// <param name="taskItem">The task item view model.</param>
    /// <returns>The result of the create task item process.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TaskItemWebApiModel taskItem)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var createdTaskItem = await this.taskItemWebApiService.CreateTaskItemAsync(taskItem);
        return this.RedirectToAction(nameof(this.Details), new { id = createdTaskItem.Id });
    }

    /// <summary>
    /// Displays the edit task item page.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <returns>The edit task item view.</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var taskItem = await this.taskItemWebApiService.GetTaskItemAsync(id);
        if (taskItem == null)
        {
            return this.NotFound();
        }

        var todoList = await this.todoListWebApiService.GetTodoListAsync(taskItem.TodoListId);
        if (todoList == null)
        {
            return this.NotFound();
        }

        if (todoList.OwnerId != this.userManager.GetUserId(this.User))
        {
            return this.Forbid();
        }

        this.ViewBag.AvailableTags = await this.tagWebApiService.GetAllTagsAsync();

        return this.View(taskItem);
    }

    /// <summary>
    /// Handles the edit task item form submission.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="taskItem">The task item view model.</param>
    /// <returns>The result of the edit task item process.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TaskItemWebApiModel taskItem)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var validationResult = this.ValidateEditParameters(id, taskItem);
        if (validationResult != null)
        {
            return validationResult;
        }

        return await this.HandleEditAsync(id, taskItem);
    }

    /// <summary>
    /// Displays the delete task item page.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <returns>The delete task item view.</returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var taskItem = await this.taskItemWebApiService.GetTaskItemAsync(id);
        if (taskItem == null)
        {
            return this.NotFound();
        }

        var todoList = await this.todoListWebApiService.GetTodoListAsync(taskItem.TodoListId);
        if (todoList == null)
        {
            return this.NotFound();
        }

        if (todoList.OwnerId != this.userManager.GetUserId(this.User))
        {
            return this.Forbid();
        }

        return this.View(taskItem);
    }

    /// <summary>
    /// Handles the delete task item form submission.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <returns>The result of the delete task item process.</returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var taskItem = await this.taskItemWebApiService.GetTaskItemAsync(id);
        if (taskItem == null)
        {
            return this.NotFound();
        }

        var todoListId = taskItem.TodoListId;
        await this.taskItemWebApiService.DeleteTaskItemAsync(id);
        return this.RedirectToAction("Details", "TodoList", new { id = todoListId });
    }

    /// <summary>
    /// Displays the assigned tasks page.
    /// </summary>
    /// <param name="status">The status of the tasks.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="sortOrder">The sort order.</param>
    /// <param name="searchString">The search string.</param>
    /// <returns>The assigned tasks view.</returns>
    [HttpGet]
    public async Task<IActionResult> Assigned(string? status = null, string? sortBy = "name", string? sortOrder = "asc", string? searchString = null)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var userId = this.userManager.GetUserId(this.User);
        var tasks = await this.taskItemWebApiService.GetAssignedTasksAsync(userId);

        if (!string.IsNullOrEmpty(status))
        {
            tasks = tasks.Where(t => t.Status.ToString().Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        tasks = sortBy switch
        {
            "duedate" => sortOrder == "desc" ? tasks.OrderByDescending(t => t.DueDate) : tasks.OrderBy(t => t.DueDate),
            _ => sortOrder == "desc" ? tasks.OrderByDescending(t => t.Title) : tasks.OrderBy(t => t.Title),
        };

        if (!string.IsNullOrEmpty(searchString))
        {
            tasks = tasks.Where(t => t.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var viewModel = new AssignedTasksViewModel
        {
            UserId = userId,
            Tasks = tasks,
            SearchString = searchString,
            Status = status,
            SortBy = sortBy,
            SortOrder = sortOrder,
        };

        return this.View(viewModel);
    }

    /// <summary>
    /// Handles the update task status form submission.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="status">The new status of the task item.</param>
    /// <returns>The result of the update task status process.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Assigned(int id, TaskStatus status)
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

        return this.RedirectToAction("Assigned", new { userId = updatedTask.UserId });
    }

    /// <summary>
    /// Handles the add tag to task form submission.
    /// </summary>
    /// <param name="taskId">The ID of the task item.</param>
    /// <param name="tagId">The ID of the tag.</param>
    /// <returns>The result of the add tag to task process.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTag(int taskId, int tagId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var taskItem = await this.taskItemWebApiService.GetTaskItemAsync(taskId);
        if (taskItem == null)
        {
            return this.NotFound();
        }

        var todoList = await this.todoListWebApiService.GetTodoListAsync(taskItem.TodoListId);
        if (todoList == null)
        {
            return this.NotFound();
        }

        if (todoList.OwnerId != this.userManager.GetUserId(this.User))
        {
            return this.Forbid();
        }

        _ = await this.tagWebApiService.AddTagToTaskAsync(taskId, tagId);
        return this.RedirectToAction(nameof(this.Details), new { id = taskId });
    }

    /// <summary>
    /// Handles the remove tag from task form submission.
    /// </summary>
    /// <param name="taskId">The ID of the task item.</param>
    /// <param name="tagId">The ID of the tag.</param>
    /// <returns>The result of the remove tag from task process.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveTag(int taskId, int tagId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var taskItem = await this.taskItemWebApiService.GetTaskItemAsync(taskId);
        if (taskItem == null)
        {
            return this.NotFound();
        }

        var todoList = await this.todoListWebApiService.GetTodoListAsync(taskItem.TodoListId);
        if (todoList == null)
        {
            return this.NotFound();
        }

        if (todoList.OwnerId != this.userManager.GetUserId(this.User))
        {
            return this.Forbid();
        }

        await this.tagWebApiService.RemoveTagFromTaskAsync(taskId, tagId);
        return this.RedirectToAction(nameof(this.Details), new { id = taskId });
    }

    /// <summary>
    /// Handles the add comment to task form submission.
    /// </summary>
    /// <param name="taskId">The ID of the task item.</param>
    /// <param name="text">The text of the comment.</param>
    /// <returns>The result of the add comment to task process.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int taskId, string text)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var taskItem = await this.taskItemWebApiService.GetTaskItemAsync(taskId);
        if (taskItem == null)
        {
            return this.NotFound();
        }

        var todoList = await this.todoListWebApiService.GetTodoListAsync(taskItem.TodoListId);
        if (todoList == null)
        {
            return this.NotFound();
        }

        if (todoList.OwnerId != this.userManager.GetUserId(this.User))
        {
            return this.Forbid();
        }

        if (string.IsNullOrEmpty(text))
        {
            this.ModelState.AddModelError("Text", "Comment text is required.");
            return this.BadRequest(this.ModelState);
        }

        var commentDto = new CommentWebApiModel { Text = text };
        _ = await this.commentWebApiService.CreateCommentAsync(taskId, commentDto);
        return this.RedirectToAction(nameof(this.Edit), new { id = taskId });
    }

    /// <summary>
    /// Handles the remove comment from task form submission.
    /// </summary>
    /// <param name="taskId">The ID of the task item.</param>
    /// <param name="commentId">The ID of the comment.</param>
    /// <returns>The result of the remove comment from task process.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveComment(int taskId, int commentId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var taskItem = await this.taskItemWebApiService.GetTaskItemAsync(taskId);
        if (taskItem == null)
        {
            return this.NotFound();
        }

        var todoList = await this.todoListWebApiService.GetTodoListAsync(taskItem.TodoListId);
        if (todoList == null)
        {
            return this.NotFound();
        }

        if (todoList.OwnerId != this.userManager.GetUserId(this.User))
        {
            return this.Forbid();
        }

        await this.commentWebApiService.DeleteCommentAsync(taskId, commentId);
        return this.RedirectToAction(nameof(this.Edit), new { id = taskId });
    }

    /// <summary>
    /// Displays the edit comment page.
    /// </summary>
    /// <param name="taskId">The ID of the task item.</param>
    /// <param name="commentId">The ID of the comment.</param>
    /// <returns>The edit comment view.</returns>
    [HttpGet]
    public async Task<IActionResult> EditComment(int taskId, int commentId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var taskItem = await this.taskItemWebApiService.GetTaskItemAsync(taskId);
        if (taskItem == null)
        {
            return this.NotFound();
        }

        var todoList = await this.todoListWebApiService.GetTodoListAsync(taskItem.TodoListId);

        if (todoList.OwnerId != this.userManager.GetUserId(this.User))
        {
            return this.Forbid();
        }

        var comment = await this.commentWebApiService.GetCommentAsync(taskId, commentId);
        if (comment == null)
        {
            return this.NotFound();
        }

        return this.View(comment);
    }

    /// <summary>
    /// Handles the edit comment form submission.
    /// </summary>
    /// <param name="comment">The comment view model.</param>
    /// <returns>The result of the edit comment process.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditComment(CommentWebApiModel comment)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var validationResult = this.ValidateEditCommentParameters(comment);
        if (validationResult != null)
        {
            return validationResult;
        }

        return await this.HandleEditCommentAsync(comment);
    }

    /// <summary>
    /// Validates the parameters for editing a task item.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="taskItem">The task item view model.</param>
    /// <returns>A <see cref="BadRequestObjectResult"/> if validation fails; otherwise, null.</returns>
    private BadRequestObjectResult? ValidateEditParameters(int id, TaskItemWebApiModel taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        if (id != taskItem.Id)
        {
            return this.BadRequest("ID mismatch.");
        }

        return null;
    }

    /// <summary>
    /// Handles the edit task item process asynchronously.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="taskItem">The task item view model.</param>
    /// <returns>The result of the edit task item process.</returns>
    private async Task<IActionResult> HandleEditAsync(int id, TaskItemWebApiModel taskItem)
    {
        var updatedTaskItem = await this.taskItemWebApiService.UpdateTaskItemAsync(id, taskItem);
        return this.RedirectToAction(nameof(this.Details), new { id = updatedTaskItem.Id });
    }

    /// <summary>
    /// Validates the parameters for editing a comment.
    /// </summary>
    /// <param name="comment">The comment view model.</param>
    /// <returns>A <see cref="ViewResult"/> if validation fails; otherwise, null.</returns>
    private ViewResult? ValidateEditCommentParameters(CommentWebApiModel comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        if (!this.ModelState.IsValid)
        {
            return this.View(comment);
        }

        return null;
    }

    /// <summary>
    /// Handles the edit comment process asynchronously.
    /// </summary>
    /// <param name="comment">The comment view model.</param>
    /// <returns>The result of the edit comment process.</returns>
    private async Task<IActionResult> HandleEditCommentAsync(CommentWebApiModel comment)
    {
        _ = await this.commentWebApiService.UpdateCommentAsync(comment.TaskItemId, comment.Id, comment);
        return this.RedirectToAction("Edit", new { id = comment.TaskItemId });
    }
}
