using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Models;

namespace TodoListApp.Services.Implementations;
public class TagDatabaseService : ITagDatabaseService
{
    private readonly TodoListDbContext context;

    public TagDatabaseService(TodoListDbContext context)
    {
        this.context = context;
    }

    public IQueryable<Tag> Tags => this.context.Tags;
}
