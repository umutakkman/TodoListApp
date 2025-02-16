namespace TodoListApp.WebApi.Models.ViewModels
{
    /// <summary>
    /// View model for displaying tag details and associated task count.
    /// </summary>
    public class TagIndexViewModel
    {
        /// <summary>
        /// Gets or sets the tag details.
        /// </summary>
        public ApiModels.TagWebApiModel Tag { get; set; } = new ApiModels.TagWebApiModel();

        /// <summary>
        /// Gets or sets the count of tasks associated with the tag.
        /// </summary>
        public int TaskCount { get; set; }
    }
}
