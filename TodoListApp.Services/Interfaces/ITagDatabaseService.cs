using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Interfaces;

/// <summary>
/// Interface for tag database service.
/// </summary>
public interface ITagDatabaseService
{
    /// <summary>
    /// Gets the collection of tags.
    /// </summary>
    IQueryable<TagEntity> Tags { get; }
}
