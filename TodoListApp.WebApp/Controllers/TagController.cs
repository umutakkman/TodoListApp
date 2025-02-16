using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ViewModels;

namespace TodoListApp.WebApp.Controllers;

/// <summary>
/// Controller for managing tags.
/// </summary>
public class TagController : Controller
{
    private readonly ITagWebApiService tagWebApiService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TagController"/> class.
    /// </summary>
    /// <param name="tagWebApiService">The tag Web API service.</param>
    public TagController(ITagWebApiService tagWebApiService)
    {
        this.tagWebApiService = tagWebApiService;
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

        var tasks = await this.tagWebApiService.GetTasksByTagAsync(tagId);
        return this.View(tasks);
    }
}
