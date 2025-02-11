using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Models;

namespace TodoListApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskItemController : ControllerBase
{
    private readonly ITaskItemDatabaseService taskItemDatabaseService;

    public TaskItemController(ITaskItemDatabaseService taskItemDatabaseService)
    {
        this.taskItemDatabaseService = taskItemDatabaseService;
    }

    [HttpGet]
    public ActionResult<TaskItem> GetTaskItems()
    {
        var items = this.taskItemDatabaseService.TaskItems.ToList();
        return this.Ok(items);
    }

    [HttpGet("{id:int}")]
    public ActionResult<TaskItem> GetTaskItemById(int id)
    {
        var item = this.taskItemDatabaseService.TaskItems.FirstOrDefault(x => x.Id == id);
        if (item == null)
        {
            return this.NotFound();
        }

        return this.Ok(item);
    }

    [HttpPost]
    public ActionResult<TaskItem> CreateTaskItem([FromBody] TaskItem taskItem)
    {
        if (taskItem == null)
        {
            return this.BadRequest("Invalid task item data.");
        }

        this.taskItemDatabaseService.CreateTaskItem(taskItem);
        return this.CreatedAtAction(nameof(this.GetTaskItemById), new { id = taskItem.Id }, taskItem);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateTaskItem(int id, [FromBody] TaskItem taskItem)
    {
        ArgumentNullException.ThrowIfNull(taskItem);
        if (id != taskItem.Id)
        {
            return this.BadRequest("ID mismatch.");
        }

        var existing = this.taskItemDatabaseService.TaskItems.FirstOrDefault(x => x.Id == id);
        if (existing == null)
        {
            return this.NotFound();
        }

        this.taskItemDatabaseService.UpdateTaskItem(taskItem);
        return this.NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteTaskItem(int id)
    {
        var existing = this.taskItemDatabaseService.TaskItems.FirstOrDefault(x => x.Id == id);
        if (existing == null)
        {
            return this.NotFound();
        }

        this.taskItemDatabaseService.DeleteTaskItem(existing);
        return this.NoContent();
    }
}
