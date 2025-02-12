using System.Net.Http.Json;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;

namespace TodoListApp.Services.Implementations;
public class TaskItemWebApiService : ITaskItemWebApiService
{
    private readonly HttpClient httpClient;

    public TaskItemWebApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<TaskItemWebApiModel> GetTaskItemAsync(int id)
    {
        var response = await this.httpClient.GetFromJsonAsync<TaskItemWebApiModel>($"api/TaskItem/{id}");
        ArgumentNullException.ThrowIfNull(response);

        return response;
    }

    public async Task<TaskItemWebApiModel> CreateTaskItemAsync(TaskItemWebApiModel taskItem)
    {
        var response = await this.httpClient.PostAsJsonAsync("api/TaskItem", taskItem);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"CreateTaskItem failed with status code {response.StatusCode}: {errorContent}");
        }

        var createdtaskItem = await response.Content.ReadFromJsonAsync<TaskItemWebApiModel>();
        ArgumentNullException.ThrowIfNull(createdtaskItem);
        return createdtaskItem;
    }

    public async Task DeleteTaskItemAsync(int id)
    {
        var response = await this.httpClient.DeleteAsync($"api/TaskItem/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<TaskItemWebApiModel> UpdateTaskItemAsync(int id, TaskItemWebApiModel taskItem)
    {
        var response = await this.httpClient.PutAsJsonAsync($"api/TaskItem/{id}", taskItem);
        response.EnsureSuccessStatusCode();
        var updatedtaskItem = await response.Content.ReadFromJsonAsync<TaskItemWebApiModel>();
        ArgumentNullException.ThrowIfNull(updatedtaskItem);
        return updatedtaskItem;
    }

    public async Task<TaskItemWebApiModel> GetAssignedTasksAsync(int userId)
    {
        var response = await this.httpClient.GetFromJsonAsync<TaskItemWebApiModel>($"api/TaskItem/Assigned/{userId}");
        ArgumentNullException.ThrowIfNull(response);
        return response;
    }

    public async Task<TaskItemWebApiModel> UpdateTaskStatusAsync(int id, TaskStatus status)
    {
        var response = await this.httpClient.PutAsJsonAsync($"api/TaskItem/Status/{id}", status);
        response.EnsureSuccessStatusCode();
        var updatedTask = await response.Content.ReadFromJsonAsync<TaskItemWebApiModel>();
        ArgumentNullException.ThrowIfNull(updatedTask);
        return updatedTask;
    }
}
