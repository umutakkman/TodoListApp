using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models.ViewModels;

namespace TodoListApp.WebApp.Controllers;
[AllowAnonymous]
public class HomeController : Controller
{
    /// <summary>
    /// Displays the home page.
    /// </summary>
    /// <returns>The home page view.</returns>
    [HttpGet]
    public IActionResult Index()
    {
        return this.View();
    }

    /// <summary>
    /// Displays the error page.
    /// </summary>
    /// <returns>The error page view.</returns>
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }
}
