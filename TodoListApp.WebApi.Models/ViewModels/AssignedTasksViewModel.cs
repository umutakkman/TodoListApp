namespace TodoListApp.WebApi.Models.ViewModels
{
    public class AssignedTasksViewModel
    {
        public IEnumerable<ApiModels.TaskItemWebApiModel> Tasks { get; set; } = new List<ApiModels.TaskItemWebApiModel>();

        public int UserId { get; set; }

        public string? SearchString { get; set; }

        public string? Status { get; set; }

        public string? SortBy { get; set; } = "name";

        public string? SortOrder { get; set; } = "asc";
    }
}
