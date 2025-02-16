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
        private readonly ITagDatabaseService tagDatabaseService;
        private readonly ICommentDatabaseService commentDatabaseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskItemController"/> class.
        /// </summary>
        /// <param name="taskItemDatabaseService">The task item database service.</param>
        /// <param name="tagDatabaseService">The tag database service.</param>
        /// <param name="commentDatabaseService">The comment database service.</param>
        public TaskItemController(ITaskItemDatabaseService taskItemDatabaseService, ITagDatabaseService tagDatabaseService, ICommentDatabaseService commentDatabaseService)
        {
            this.taskItemDatabaseService = taskItemDatabaseService;
            this.tagDatabaseService = tagDatabaseService;
            this.commentDatabaseService = commentDatabaseService;
        }

        /// <summary>
        /// Gets a task item by ID.
        /// </summary>
        /// <param name="id">The ID of the task item.</param>
        /// <returns>The task item.</returns>
        [HttpGet("{id:int}")]
        public ActionResult<TaskItemWebApiModel> GetTaskItemById(int id)
        {
            var entity = this.taskItemDatabaseService.TaskItems
                        .Include(t => t.Tags)
                        .Include(t => t.Comments)
                        .FirstOrDefault(t => t.Id == id);

            if (entity == null)
            {
                return this.NotFound();
            }

            ArgumentNullException.ThrowIfNull(entity);
            var dto = MapEntityToDto(entity);
            return this.Ok(dto);
        }

        /// <summary>
        /// Creates a new task item.
        /// </summary>
        /// <param name="dto">The task item DTO.</param>
        /// <returns>The created task item.</returns>
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

        /// <summary>
        /// Updates an existing task item.
        /// </summary>
        /// <param name="id">The ID of the task item.</param>
        /// <param name="dto">The task item DTO.</param>
        /// <returns>The updated task item.</returns>
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

        /// <summary>
        /// Deletes a task item.
        /// </summary>
        /// <param name="id">The ID of the task item.</param>
        /// <returns>No content.</returns>
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

        /// <summary>
        /// Gets the tasks assigned to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The collection of assigned tasks.</returns>
        [HttpGet("assigned/{userId}")]
        public ActionResult<IEnumerable<TaskItemWebApiModel>> GetAssignedTasks(string userId)
        {
            var entities = this.taskItemDatabaseService.TaskItems.Where(x => x.UserId == userId).ToList();
            var dtos = entities.Select(MapEntityToDto);
            return this.Ok(dtos);
        }

        /// <summary>
        /// Updates the status of a task item.
        /// </summary>
        /// <param name="id">The ID of the task item.</param>
        /// <param name="model">The change status view model.</param>
        /// <returns>No content.</returns>
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

        /// <summary>
        /// Gets the tags for a specific task.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>The collection of tags for the task.</returns>
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

        /// <summary>
        /// Gets the tasks by a specific tag.
        /// </summary>
        /// <param name="tagId">The ID of the tag.</param>
        /// <returns>The collection of tasks associated with the tag.</returns>
        [ProducesResponseType(typeof(IEnumerable<TaskItemWebApiModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("bytag/{tagId:int}")]
        public IActionResult GetTasksByTag(int tagId)
        {
            var entities = this.taskItemDatabaseService.TaskItems
                            .Include(t => t.Tags)
                            .AsEnumerable()
                            .Where(t => t.Tags != null && t.Tags.Any(tag => tag.Id == tagId))
                            .ToList();
            var dtos = entities.Select(MapEntityToDto);
            return this.Ok(dtos);
        }

        /// <summary>
        /// Adds a tag to a task.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <param name="tagId">The ID of the tag.</param>
        /// <returns>The updated collection of tags for the task.</returns>
        [ProducesResponseType(typeof(IEnumerable<TagWebApiModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("{taskId:int}/tags")]
        public IActionResult AddTagToTask(int taskId, [FromBody] int tagId)
        {
            var task = this.taskItemDatabaseService.TaskItems
                        .Include(t => t.Tags)
                        .FirstOrDefault(t => t.Id == taskId);

            if (task == null)
            {
                return this.NotFound();
            }

            var tag = this.tagDatabaseService.Tags.FirstOrDefault(t => t.Id == tagId);

            if (tag == null)
            {
                return this.BadRequest("Tag not found.");
            }

            if (task.Tags == null)
            {
                task.Tags = new List<TagEntity>();
            }

            if (!task.Tags.Any(t => t.Id == tagId))
            {
                task.Tags.Add(tag);
                this.taskItemDatabaseService.UpdateTaskItem(task);
            }

            var tagDtos = task.Tags.Select(t => new TagWebApiModel
            {
                Id = t.Id,
                Name = t.Name,
            });

            return this.Ok(tagDtos);
        }

        /// <summary>
        /// Removes a tag from a task.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <param name="tagId">The ID of the tag.</param>
        /// <returns>No content.</returns>
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

        /// <summary>
        /// Gets a comment for a specific task.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <param name="commentId">The ID of the comment.</param>
        /// <returns>The comment.</returns>
        [ProducesResponseType(typeof(IEnumerable<CommentWebApiModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{taskId:int}/comments/{commentId:int}")]
        public IActionResult GetComment(int taskId, int commentId)
        {
            var task = this.taskItemDatabaseService.TaskItems
                        .Include(t => t.Comments)
                        .FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return this.NotFound();
            }

            if (task.Comments == null || task.Comments.Count == 0)
            {
                return this.NotFound("Comments not found.");
            }

            var comment = task.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return this.NotFound();
            }

            var commentDto = new CommentWebApiModel
            {
                Id = comment.Id,
                Text = comment.Text,
                CreationDate = comment.CreationDate,
                TaskItemId = comment.TaskItemId,
                UserId = comment.UserId,
            };
            return this.Ok(commentDto);
        }

        /// <summary>
        /// Adds a comment to a task.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <param name="dto">The comment DTO.</param>
        /// <returns>The created comment.</returns>
        [ProducesResponseType(typeof(CommentEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("{taskId:int}/comments")]
        public IActionResult AddCommentToTask(int taskId, [FromBody] CommentWebApiModel dto)
        {
            if (dto == null)
            {
                return this.BadRequest("Invalid comment data.");
            }

            var task = this.taskItemDatabaseService.TaskItems
                        .Include(t => t.Comments)
                        .FirstOrDefault(t => t.Id == taskId);

            if (task == null)
            {
                return this.NotFound();
            }

            var comment = new CommentEntity
            {
                Text = dto.Text,
                TaskItemId = taskId,
                CreationDate = DateTime.Now,
                UserId = dto.UserId,
            };

            this.commentDatabaseService.CreateComment(comment);

            var commentDto = new CommentWebApiModel
            {
                Id = comment.Id,
                Text = comment.Text,
                CreationDate = comment.CreationDate,
                TaskItemId = comment.TaskItemId,
                UserId = comment.UserId,
            };

            return this.Ok(commentDto);
        }

        /// <summary>
        /// Updates a comment on a task.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <param name="commentId">The ID of the comment.</param>
        /// <param name="dto">The comment DTO.</param>
        /// <returns>The updated comment.</returns>
        [ProducesResponseType(typeof(CommentWebApiModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{taskId:int}/comments/{commentId:int}")]
        public IActionResult UpdateCommentOnTask(int taskId, int commentId, [FromBody] CommentWebApiModel dto)
        {
            if (dto == null)
            {
                return this.BadRequest("Invalid comment data.");
            }

            var task = this.taskItemDatabaseService.TaskItems
                        .Include(t => t.Comments)
                        .FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return this.NotFound("Task not found.");
            }

            if (task.Comments == null)
            {
                return this.NotFound("Comments not found.");
            }

            var existingComment = task.Comments.FirstOrDefault(c => c.Id == commentId);
            if (existingComment == null)
            {
                return this.NotFound("Comment not found.");
            }

            existingComment.Text = dto.Text;

            this.commentDatabaseService.UpdateComment(existingComment);

            var updatedDto = new CommentWebApiModel
            {
                Id = existingComment.Id,
                Text = existingComment.Text,
                CreationDate = existingComment.CreationDate,
                TaskItemId = existingComment.TaskItemId,
                UserId = existingComment.UserId,
            };

            return this.Ok(updatedDto);
        }

        /// <summary>
        /// Deletes a comment from a task.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <param name="commentId">The ID of the comment.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{taskId:int}/comments/{commentId:int}")]
        public IActionResult DeleteCommentFromTask(int taskId, int commentId)
        {
            var task = this.taskItemDatabaseService.TaskItems
                        .Include(t => t.Comments)
                        .FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                return this.NotFound("Task not found.");
            }

            if (task.Comments == null)
            {
                return this.NotFound("Comments not found.");
            }

            var comment = task.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return this.NotFound("Comment not found.");
            }

            this.commentDatabaseService.DeleteComment(comment);
            return this.NoContent();
        }

        /// <summary>
        /// Maps a task item entity to a task item DTO.
        /// </summary>
        /// <param name="entity">The task item entity.</param>
        /// <returns>The task item DTO.</returns>
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
                Comments = entity.Comments?.Select(c => new CommentWebApiModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    CreationDate = c.CreationDate,
                    TaskItemId = c.TaskItemId,
                    UserId = c.UserId,
                }).ToList() ?? new List<CommentWebApiModel>(),
            };
        }

        /// <summary>
        /// Maps a task item DTO to a task item entity.
        /// </summary>
        /// <param name="dto">The task item DTO.</param>
        /// <returns>The task item entity.</returns>
        private static TaskItemEntity MapDtoToEntity(TaskItemWebApiModel dto)
        {
            return new TaskItemEntity
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Status = dto.Status,
                UserId = dto.UserId,
                TodoListId = dto.TodoListId,
            };
        }
    }
}
