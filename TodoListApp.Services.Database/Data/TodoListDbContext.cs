using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Models.Entities;
using TodoListApp.WebApi.Models.Models;

namespace TodoListApp.Services.Database.Data;

public class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoList> TodoLists { get; set; } = null!;

    public DbSet<TaskItem> TaskItems { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Comment> Comments { get; set; } = null!;

    public DbSet<Tag> Tags { get; set; } = null!;
}
