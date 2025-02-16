using TodoListApp.WebApi.Models.ApiModels;
using TaskStatus = TodoListApp.Common.TaskStatus;

namespace TodoListApp.Services.Interfaces;
public interface ITaskItemWebApiService
{
    Task<TaskItemWebApiModel> GetTaskItemAsync(int id);

    Task<TaskItemWebApiModel> CreateTaskItemAsync(TaskItemWebApiModel taskItem);

    Task DeleteTaskItemAsync(int id);

    Task<TaskItemWebApiModel> UpdateTaskItemAsync(int id, TaskItemWebApiModel taskItem);

    Task<IEnumerable<TaskItemWebApiModel>> GetAssignedTasksAsync(string userId);

    Task<TaskItemWebApiModel> UpdateTaskStatusAsync(int id, TaskStatus status);
}
