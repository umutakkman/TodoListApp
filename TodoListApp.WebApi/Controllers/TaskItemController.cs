using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;
using TodoListApp.WebApi.Models.Entities;
using TodoListApp.WebApi.Models.ViewModels;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskItemController : ControllerBase
    {
        private readonly ITaskItemDatabaseService taskItemDatabaseService;

        public TaskItemController(ITaskItemDatabaseService taskItemDatabaseService)
        {
            this.taskItemDatabaseService = taskItemDatabaseService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<TaskItemWebApiModel> GetTaskItemById(int id)
        {
            var entity = this.taskItemDatabaseService.TaskItems.FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                return this.NotFound();
            }

            var dto = MapEntityToDto(entity);
            return this.Ok(dto);
        }

        [HttpPost]
        public ActionResult<TaskItemWebApiModel> CreateTaskItem([FromBody] TaskItemWebApiModel dto)
        {
            if (dto == null)
            {
                return this.BadRequest("Invalid task item data.");
            }

            var entity = MapDtoToEntity(dto);
            this.taskItemDatabaseService.CreateTaskItem(entity);
            var createdDto = MapEntityToDto(entity);
            return this.CreatedAtAction(nameof(this.GetTaskItemById), new { id = createdDto.Id }, createdDto);
        }

        [ProducesResponseType(typeof(TaskItemWebApiModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:int}")]
        public IActionResult UpdateTaskItem(int id, [FromBody] TaskItemWebApiModel dto)
        {
            if (dto == null)
            {
                return this.BadRequest("Invalid task item data.");
            }

            if (id != dto.Id)
            {
                return this.BadRequest("ID mismatch.");
            }

            var existing = this.taskItemDatabaseService.TaskItems.FirstOrDefault(x => x.Id == id);
            if (existing == null)
            {
                return this.NotFound();
            }

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.DueDate = dto.DueDate;
            existing.Status = dto.Status;

            this.taskItemDatabaseService.UpdateTaskItem(existing);

            var updatedEntity = this.taskItemDatabaseService.TaskItems.FirstOrDefault(x => x.Id == id);
            if (updatedEntity == null)
            {
                return this.NotFound();
            }

            var updatedDto = MapEntityToDto(updatedEntity);
            return this.Ok(updatedDto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteTaskItem(int id)
        {
            var existing = this.taskItemDatabaseService.TaskItems.FirstOrDefault(x => x.Id == id);
            if (existing == null)
            {
                return this.NotFound();
            }

            this.taskItemDatabaseService.DeleteTaskItem(existing);
            return this.NoContent();
        }

        [HttpGet("assigned/{userId:int}")]
        public ActionResult<IEnumerable<TaskItemWebApiModel>> GetAssignedTasks(int userId)
        {
            var entities = this.taskItemDatabaseService.TaskItems.Where(x => x.UserId == userId).ToList();
            var dtos = entities.Select(MapEntityToDto);
            return this.Ok(dtos);
        }

        [HttpPut("status/{id:int}")]
        public IActionResult UpdateTaskStatus(int id, [FromBody] ChangeStatusViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            ArgumentNullException.ThrowIfNull(model);

            if (model.Id != id)
            {
                return this.BadRequest("ID mismatch.");
            }

            var existing = this.taskItemDatabaseService.TaskItems.FirstOrDefault(x => x.Id == id);
            if (existing == null)
            {
                return this.NotFound();
            }

            existing.Status = model.TaskStatus;
            this.taskItemDatabaseService.UpdateTaskItem(existing);
            return this.NoContent();
        }

        [ProducesResponseType(typeof(IEnumerable<TagWebApiModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{taskId:int}/tags")]
        public IActionResult GetTagsForTask(int taskId)
        {
            var task = this.taskItemDatabaseService.TaskItems
                        .Include(t => t.Tags)
                        .FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return this.NotFound();
            }

            var tagDtos = task.Tags?.Select(t => new TagWebApiModel { Id = t.Id, Name = t.Name }) ?? Enumerable.Empty<TagWebApiModel>();
            return this.Ok(tagDtos);
        }

        [ProducesResponseType(typeof(IEnumerable<TaskItemWebApiModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("bytag/{tagId:int}")]
        public IActionResult GetTasksByTag(int tagId)
        {
            var entities = this.taskItemDatabaseService.TaskItems
                            .Where(t => t.Tags != null && t.Tags.Any(tag => tag.Id == tagId))
                            .ToList();
            var dtos = entities.Select(MapEntityToDto);
            return this.Ok(dtos);
        }

        [ProducesResponseType(typeof(IEnumerable<TagWebApiModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("{taskId:int}/tags")]
        public IActionResult AddTagToTask(int taskId, [FromBody] TagWebApiModel tagDto)
        {
            var task = this.taskItemDatabaseService.TaskItems
                        .Include(t => t.Tags)
                        .FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return this.NotFound();
            }

            if (tagDto == null)
            {
                return this.BadRequest("Invalid tag data.");
            }

            if (task.Tags == null)
            {
                task.Tags = new List<TagEntity>();
            }

            var newTag = new TagEntity
            {
                Name = tagDto.Name,
            };
            task.Tags.Add(newTag);
            this.taskItemDatabaseService.UpdateTaskItem(task);
            var tagDtos = task.Tags.Select(t => new TagWebApiModel { Id = t.Id, Name = t.Name });
            return this.Ok(tagDtos);
        }

        [HttpDelete("{taskId:int}/tag/{tagId:int}")]
        public IActionResult RemoveTagFromTask(int taskId, int tagId)
        {
            var task = this.taskItemDatabaseService.TaskItems
                        .Include(t => t.Tags)
                        .FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return this.NotFound();
            }

            if (task.Tags == null)
            {
                return this.NotFound();
            }

            var tagToRemove = task.Tags.FirstOrDefault(t => t.Id == tagId);
            if (tagToRemove == null)
            {
                return this.NotFound();
            }

            _ = task.Tags.Remove(tagToRemove);
            this.taskItemDatabaseService.UpdateTaskItem(task);
            return this.NoContent();
        }

        private static TaskItemWebApiModel MapEntityToDto(TaskItemEntity entity)
        {
            return new TaskItemWebApiModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                CreationDate = entity.CreationDate,
                DueDate = entity.DueDate,
                Status = entity.Status,
                UserId = entity.UserId,
                TodoListId = entity.TodoListId,
                Tags = entity.Tags?.Select(t => new TagWebApiModel
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList() ?? new List<TagWebApiModel>(),
            };
        }

        private static TaskItemEntity MapDtoToEntity(TaskItemWebApiModel dto)
        {
            return new TaskItemEntity
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Status = dto.Status,
                UserId = dto.UserId ?? 0,
                TodoListId = dto.TodoListId,
            };
        }
    }
}
