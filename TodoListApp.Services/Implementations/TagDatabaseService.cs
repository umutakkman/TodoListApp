using TodoListApp.Services.Database.Data;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Implementations;
public class TagDatabaseService : ITagDatabaseService
{
    private readonly ApplicationDbContext context;

    public TagDatabaseService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IQueryable<TagEntity> Tags => this.context.Tag;
}
