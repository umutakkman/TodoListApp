namespace TodoListApp.WebApi.Models.ViewModels
{
    /// <summary>
    /// View model for assigned tasks.
    /// </summary>
    public class AssignedTasksViewModel
    {
        /// <summary>
        /// Gets or sets the collection of assigned tasks.
        /// </summary>
        public IEnumerable<ApiModels.TaskItemWebApiModel> Tasks { get; set; } = new List<ApiModels.TaskItemWebApiModel>();

        /// <summary>
        /// Gets or sets the ID of the user.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the search string.
        /// </summary>
        public string? SearchString { get; set; }

        /// <summary>
        /// Gets or sets the status of the tasks.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Gets or sets the field to sort by.
        /// </summary>
        public string? SortBy { get; set; } = "name";

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        public string? SortOrder { get; set; } = "asc";
    }
}
