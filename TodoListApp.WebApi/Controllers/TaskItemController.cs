using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Models;
using TodoListApp.WebApi.Models.ViewModels;
using TaskStatus = TodoListApp.Common.TaskStatus;

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
    public async Task<IActionResult> GetAssignedTasks(int userId)
    {
        var tasks = await this.taskItemDatabaseService.TaskItems
            .Where(x => x.UserId == userId)
            .ToListAsync();

        return this.Ok(tasks);
    }

    [HttpPut("status/{id:int}")]
    public IActionResult UpdateTaskStatus(int id, [FromBody] ChangeStatusViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        ArgumentNullException.ThrowIfNull(model);

        if (model.Id != id)
        {
            return this.BadRequest("ID mismatch.");
        }

        var existing = this.taskItemDatabaseService.TaskItems.FirstOrDefault(x => x.Id == id);
        if (existing == null)
        {
            return this.NotFound();
        }

        existing.Status = model.TaskStatus;
        this.taskItemDatabaseService.UpdateTaskItem(existing);
        return this.NoContent();
    }
}
