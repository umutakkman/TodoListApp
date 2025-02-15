namespace TodoListApp.WebApi.Models.ViewModels
{
    public class TagIndexViewModel
    {
        public ApiModels.TagWebApiModel Tag { get; set; } = new ApiModels.TagWebApiModel();

        public int TaskCount { get; set; }
    }
}
