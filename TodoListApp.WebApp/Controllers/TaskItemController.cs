using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;
using TodoListApp.WebApi.Models.ViewModels;
using TaskStatus = TodoListApp.Common.TaskStatus;

namespace TodoListApp.WebApp.Controllers;
public class TaskItemController : Controller
{
    private readonly ITaskItemWebApiService taskItemWebApiService;
    private readonly ITagWebApiService tagWebApiService;

    public TaskItemController(ITaskItemWebApiService taskItemWebApiService, ITagWebApiService tagWebApiService)
    {
        this.taskItemWebApiService = taskItemWebApiService;
        this.tagWebApiService = tagWebApiService;
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

        return this.View(taskItem);
    }

    public IActionResult Create(int todoListId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

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

    public async Task<IActionResult> Assigned(int userId, string? status = null, string? sortBy = "name", string? sortOrder = "asc", string? searchString = null)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

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

        await this.tagWebApiService.RemoveTagFromTaskAsync(taskId, tagId);
        return this.RedirectToAction(nameof(this.Details), new { id = taskId });
    }
}
