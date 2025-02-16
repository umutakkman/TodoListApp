using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;
using TodoListApp.WebApi.Models.ViewModels;
using TaskStatus = TodoListApp.Common.TaskStatus;

namespace TodoListApp.WebApp.Controllers;
public class TaskItemController : Controller
{
    private readonly ITaskItemWebApiService taskItemWebApiService;
    private readonly ITagWebApiService tagWebApiService;
    private readonly ICommentWebApiService commentWebApiService;
    private readonly ITodoListWebApiService todoListWebApiService;
    private readonly UserManager<IdentityUser> userManager;

    public TaskItemController(ITaskItemWebApiService taskItemWebApiService, ITagWebApiService tagWebApiService, ICommentWebApiService commentWebApiService, ITodoListWebApiService todoListWebApiService, UserManager<IdentityUser> userManager)
    {
        this.taskItemWebApiService = taskItemWebApiService;
        this.tagWebApiService = tagWebApiService;
        this.commentWebApiService = commentWebApiService;
        this.todoListWebApiService = todoListWebApiService;
        this.userManager = userManager;
    }

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

        if (taskItem.UserId != this.userManager.GetUserId(this.User) && todoList.OwnerId != this.userManager.GetUserId(this.User))
        {
            return this.Forbid();
        }

        return this.View(taskItem);
    }

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TaskItemWebApiModel taskItem)
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

        var updatedTaskItem = await this.taskItemWebApiService.UpdateTaskItemAsync(id, taskItem);
        return this.RedirectToAction(nameof(this.Details), new { id = updatedTaskItem.Id });
    }

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditComment(CommentWebApiModel comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        if (!this.ModelState.IsValid)
        {
            return this.View(comment);
        }

        _ = await this.commentWebApiService.UpdateCommentAsync(comment.TaskItemId, comment.Id, comment);
        return this.RedirectToAction("Edit", new { id = comment.TaskItemId });
    }
}
