using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Database.Data;

/// <summary>
/// The database context for the application, including identity and application-specific entities.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the to-do lists.
    /// </summary>
    public DbSet<TodoListEntity> TodoList { get; set; } = null!;

    /// <summary>
    /// Gets or sets the task items.
    /// </summary>
    public DbSet<TaskItemEntity> TaskItem { get; set; } = null!;

    /// <summary>
    /// Gets or sets the comments.
    /// </summary>
    public DbSet<CommentEntity> Comment { get; set; } = null!;

    /// <summary>
    /// Gets or sets the tags.
    /// </summary>
    public DbSet<TagEntity> Tag { get; set; } = null!;
}
