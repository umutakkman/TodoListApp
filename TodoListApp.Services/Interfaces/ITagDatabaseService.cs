using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Interfaces;
public interface ITagDatabaseService
{
    IQueryable<TagEntity> Tags { get; }
}
