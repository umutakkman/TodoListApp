using Microsoft.EntityFrameworkCore;

namespace TodoListApp.WebApi.Data;

internal class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }
}
