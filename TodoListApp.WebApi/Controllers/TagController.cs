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

        /// <summary>
        /// Initializes a new instance of the <see cref="TagController"/> class.
        /// </summary>
        /// <param name="tagDatabaseService">The tag database service.</param>
        public TagController(ITagDatabaseService tagDatabaseService)
        {
            this.tagDatabaseService = tagDatabaseService;
        }

        /// <summary>
        /// Gets the collection of tags.
        /// </summary>
        /// <returns>The collection of tags.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<TagWebApiModel>> GetTags()
        {
            var tagEntities = this.tagDatabaseService.Tags.ToList();

            var tagDtos = tagEntities.Select(MapEntityToDto);

            return this.Ok(tagDtos);
        }

        /// <summary>
        /// Maps a tag entity to a tag DTO.
        /// </summary>
        /// <param name="entity">The tag entity.</param>
        /// <returns>The tag DTO.</returns>
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
