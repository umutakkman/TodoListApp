using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Implementations;
public class TagDatabaseService : ITagDatabaseService
{
    private readonly TodoListDbContext context;

    public TagDatabaseService(TodoListDbContext context)
    {
        this.context = context;
    }

    public IQueryable<TagEntity> Tags => this.context.Tag;
}
