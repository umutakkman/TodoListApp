using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagDatabaseService tagDatabaseService;

        public TagController(ITagDatabaseService tagDatabaseService)
        {
            this.tagDatabaseService = tagDatabaseService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TagWebApiModel>> GetTags()
        {
            var tagEntities = this.tagDatabaseService.Tags.ToList();

            var tagDtos = tagEntities.Select(MapEntityToDto);

            return this.Ok(tagDtos);
        }

        private static TagWebApiModel MapEntityToDto(TagEntity entity)
        {
            return new TagWebApiModel
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }
}
