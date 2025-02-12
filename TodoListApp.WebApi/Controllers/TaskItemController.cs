using Microsoft.AspNetCore.Mvc;
using TodoListApp.Common;
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

        var updatedEntity = this.taskItemDatabaseService.TaskItems.FirstOrDefault(x => x.Id == id);
        return this.Ok(updatedEntity);
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

    [HttpGet("assigned/{currentUserId:int}")]
    public ActionResult<IEnumerable<TaskItem>> GetAssignedTasks(
        int currentUserId,
        [FromQuery] string? status = null,
        [FromQuery] string? sortBy = "name",
        [FromQuery] string? sortOrder = "asc")
    {
        var query = this.taskItemDatabaseService.TaskItems.Where(x => x.UserId == currentUserId);

        if (string.IsNullOrEmpty(status) || status.ToUpperInvariant().Equals("ACTIVE", StringComparison.Ordinal))
        {
            query = query.Where(x => !x.Status.IsCompleted());
        }

        query = sortBy?.ToUpperInvariant() switch
        {
            "duedate" => sortOrder?.ToUpperInvariant() == "desc"
                            ? query.OrderByDescending(t => t.DueDate)
                            : query.OrderBy(t => t.DueDate),
            "name" or _ => sortOrder?.ToUpperInvariant() == "desc"
                            ? query.OrderByDescending(t => t.Title)
                            : query.OrderBy(t => t.Title)
        };

        var tasks = query.ToList();
        return this.Ok(tasks);
    }

    [HttpPut("{id:int}/status")]
    public IActionResult UpdateTaskStatus(int id, [FromBody] Common.TaskStatus status)
    {
        var existing = this.taskItemDatabaseService.TaskItems.FirstOrDefault(x => x.Id == id);
        if (existing == null)
        {
            return this.NotFound();
        }

        existing.Status = status;
        this.taskItemDatabaseService.UpdateTaskItem(existing);
        return this.NoContent();
    }
}
