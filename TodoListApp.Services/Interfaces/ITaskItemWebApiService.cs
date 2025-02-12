using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.Services.Interfaces;
public interface ITaskItemWebApiService
{
    Task<TaskItemWebApiModel> GetTaskItemAsync(int id);

    Task<TaskItemWebApiModel> CreateTaskItemAsync(TaskItemWebApiModel taskItem);

    Task DeleteTaskItemAsync(int id);

    Task<TaskItemWebApiModel> UpdateTaskItemAsync(int id, TaskItemWebApiModel taskItem);
}
