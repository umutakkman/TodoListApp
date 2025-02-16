using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Services.Interfaces;
using TodoListApp.WebApi.Models.ApiModels;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoListController : ControllerBase
    {
        private readonly ITodoListDatabaseService todoListDatabaseService;

        public TodoListController(ITodoListDatabaseService todoListDatabaseService)
        {
            this.todoListDatabaseService = todoListDatabaseService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoListWebApiModel>> GetTodoLists()
        {
            var entities = this.todoListDatabaseService.TodoLists
                             .Include(x => x.TaskItems)
                             .ToList();

            var dtos = entities.Select(e => MapEntityToDto(e));
            return this.Ok(dtos);
        }

        [HttpGet("{id:int}")]
        public ActionResult<TodoListWebApiModel> GetTodoListById(int id)
        {
            var entity = this.todoListDatabaseService.TodoLists
                           .Include(x => x.TaskItems)
                           .FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                return this.NotFound();
            }

            var dto = MapEntityToDto(entity);
            return this.Ok(dto);
        }

        [HttpPost]
        public ActionResult<TodoListWebApiModel> CreateTodoList(TodoListWebApiModel dto)
        {
            if (dto == null)
            {
                return this.BadRequest("Invalid todo list data.");
            }

            var entity = MapDtoToEntity(dto);

            this.todoListDatabaseService.CreateTodoList(entity);

            var createdDto = MapEntityToDto(entity);
            return this.CreatedAtAction(nameof(this.GetTodoListById), new { id = createdDto.Id }, createdDto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(TodoListWebApiModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateTodoList(int id, [FromBody] TodoListWebApiModel dto)
        {
            if (dto == null)
            {
                return this.BadRequest("Invalid todo list data.");
            }

            if (id != dto.Id)
            {
                return this.BadRequest("ID mismatch.");
            }

            var existingEntity = this.todoListDatabaseService.TodoLists.FirstOrDefault(x => x.Id == id);
            if (existingEntity == null)
            {
                return this.NotFound();
            }

            existingEntity.Title = dto.Title;
            existingEntity.Description = dto.Description ?? string.Empty;

            this.todoListDatabaseService.UpdateTodoList(existingEntity);

            var updatedEntity = this.todoListDatabaseService.TodoLists.FirstOrDefault(x => x.Id == id);
            if (updatedEntity == null)
            {
                return this.NotFound();
            }

            var updatedDto = MapEntityToDto(updatedEntity);
            return this.Ok(updatedDto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteTodoList(int id)
        {
            var entity = this.todoListDatabaseService.TodoLists.FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                return this.NotFound();
            }

            this.todoListDatabaseService.DeleteTodoList(entity);
            return this.NoContent();
        }

        private static TodoListWebApiModel MapEntityToDto(TodoListEntity entity)
        {
            return new TodoListWebApiModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                OwnerId = entity.OwnerId,
                TaskItems = entity.TaskItems?.Select(t => new TaskItemWebApiModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    CreationDate = t.CreationDate,
                    DueDate = t.DueDate,
                    Status = t.Status,
                    UserId = t.UserId,
                    TodoListId = t.TodoListId,
                }).ToList() ?? new List<TaskItemWebApiModel>(),
            };
        }

        private static TodoListEntity MapDtoToEntity(TodoListWebApiModel dto)
        {
            return new TodoListEntity
            {
                Title = dto.Title,
                Description = dto.Description ?? string.Empty,
                OwnerId = dto.OwnerId,
            };
        }
    }
}
