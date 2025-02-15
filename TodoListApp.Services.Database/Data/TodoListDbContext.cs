using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Database.Data;

public class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoListEntity> TodoList { get; set; } = null!;

    public DbSet<TaskItemEntity> TaskItem { get; set; } = null!;

    public DbSet<UserEntity> User { get; set; } = null!;

    public DbSet<CommentEntity> Comment { get; set; } = null!;

    public DbSet<TagEntity> Tag { get; set; } = null!;
}
