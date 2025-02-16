using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Database.Data;
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoListEntity> TodoList { get; set; } = null!;

    public DbSet<TaskItemEntity> TaskItem { get; set; } = null!;

    public DbSet<CommentEntity> Comment { get; set; } = null!;

    public DbSet<TagEntity> Tag { get; set; } = null!;
}
