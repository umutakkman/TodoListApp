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

    public DbSet<UserEntity> Users { get; set; } = null!;

    public DbSet<CommentEntity> Comments { get; set; } = null!;

    public DbSet<TagEntity> Tags { get; set; } = null!;
}
