using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.WebApp.Controllers;
public class TaskItemController : Controller
{
    private readonly ITaskItemWebApiService taskItemWebApiService;

    public TaskItemController(ITaskItemWebApiService taskItemWebApiService)
    {
        this.taskItemWebApiService = taskItemWebApiService;
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

        return this.View(taskItem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TaskItemWebApiModel taskItem)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
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
}
