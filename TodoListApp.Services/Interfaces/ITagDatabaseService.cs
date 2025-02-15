using TodoListApp.WebApi.Models.Models;

namespace TodoListApp.Services.Interfaces;
public interface ITagDatabaseService
{
    IQueryable<Tag> Tags { get; }
}
