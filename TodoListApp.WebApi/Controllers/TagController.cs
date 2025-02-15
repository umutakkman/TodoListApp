using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Models;

namespace TodoListApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly ITagDatabaseService tagDatabaseService;

    public TagController(ITagDatabaseService tagDatabaseService)
    {
        this.tagDatabaseService = tagDatabaseService;
    }

    [HttpGet]
    public ActionResult<Tag> GetTags()
    {
        var tags = this.tagDatabaseService.Tags.ToList();
        return this.Ok(tags);
    }
}
