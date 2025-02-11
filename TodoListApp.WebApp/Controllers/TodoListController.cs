using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers;
public class TodoListController : Controller
{
    private readonly ITodoListWebApiService todoListWebApiService;

    public TodoListController(ITodoListWebApiService todoListWebApiService)
    {
        this.todoListWebApiService = todoListWebApiService;
    }

    public async Task<IActionResult> Index()
    {
        var todoLists = await this.todoListWebApiService.GetTodoListsAsync();
        return this.View(todoLists);
    }
}
