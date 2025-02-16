using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Implementations;

/// <summary>
/// Service for managing tags in the database.
/// </summary>
public class TagDatabaseService : ITagDatabaseService
{
    private readonly ApplicationDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TagDatabaseService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public TagDatabaseService(ApplicationDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets the collection of tags.
    /// </summary>
    public IQueryable<TagEntity> Tags => this.context.Tag;
}
