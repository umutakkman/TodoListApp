using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ViewModels;
using TaskStatus = TodoListApp.Common.TaskStatus;

namespace TodoListApp.WebApp.Controllers;

/// <summary>
/// Controller for managing tags.
/// </summary>
public class TagController : Controller
{
    private readonly ITagWebApiService tagWebApiService;
    private readonly ITaskItemWebApiService taskItemWebApiService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TagController"/> class.
    /// </summary>
    /// <param name="tagWebApiService">The tag Web API service.</param>
    public TagController(ITagWebApiService tagWebApiService, ITaskItemWebApiService taskItemWebApiService)
    {
        this.tagWebApiService = tagWebApiService;
        this.taskItemWebApiService = taskItemWebApiService;
    }

    /// <summary>
    /// Displays the list of tags.
    /// </summary>
    /// <returns>The view with the list of tags.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var tags = await this.tagWebApiService.GetAllTagsAsync();
        var viewModelList = new List<TagIndexViewModel>();

        foreach (var tag in tags)
        {
            var tasks = await this.tagWebApiService.GetTasksByTagAsync(tag.Id);
            if (tasks.Any())
            {
                viewModelList.Add(new TagIndexViewModel
                {
                    Tag = tag,
                    TaskCount = tasks.Count(),
                });
            }
        }

        return this.View(viewModelList);
    }

    /// <summary>
    /// Displays the tasks associated with a specific tag.
    /// </summary>
    /// <param name="tagId">The ID of the tag.</param>
    /// <returns>The view with the list of tasks associated with the tag.</returns>
    [HttpGet]
    public async Task<IActionResult> TasksByTag(int tagId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        // Get all tags to find the name of the current tag
        var allTags = await this.tagWebApiService.GetAllTagsAsync();
        var currentTag = allTags.FirstOrDefault(t => t.Id == tagId);

        // Set the tag name in ViewBag
        this.ViewBag.TagName = currentTag?.Name ?? "Unknown Tag";

        var tasks = await this.tagWebApiService.GetTasksByTagAsync(tagId);
        return this.View(tasks);
    }

    /// <summary>
    /// Updates the status of a task and redirects back to the TasksByTag view.
    /// </summary>
    /// <param name="id">The ID of the task item.</param>
    /// <param name="status">The new status of the task item.</param>
    /// <param name="tagId">The ID of the tag to redirect back to.</param>
    /// <returns>The result of the update task status process.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatusForTag(int id, TaskStatus status, int tagId)
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

        return this.RedirectToAction("TasksByTag", "Tag", new { tagId = tagId });
    }
}
