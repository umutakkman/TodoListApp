using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ViewModels;

namespace TodoListApp.WebApp.Controllers;
public class TagController : Controller
{
    private readonly ITagWebApiService tagWebApiService;

    public TagController(ITagWebApiService tagWebApiService)
    {
        this.tagWebApiService = tagWebApiService;
    }

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
