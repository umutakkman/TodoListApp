using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Models.Models;

namespace TodoListApp.Services.Database.Data;

public class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoList> TodoLists { get; set; } = null!;
}
