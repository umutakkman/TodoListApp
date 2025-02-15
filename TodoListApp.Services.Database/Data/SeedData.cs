using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Database.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new TodoListDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<TodoListDbContext>>());
            if (context.User.Any() && context.Tag.Any())
            {
                return;
            }

            if (!context.User.Any())
            {
                var defaultUser = new UserEntity
                {
                    Username = "admin",
                    Password = "password",
                };

                _ = context.User.Add(defaultUser);
            }

            if (!context.Tag.Any())
            {
                context.Tag.AddRange(
                    new TagEntity { Name = "High Priority" },
                    new TagEntity { Name = "Normal Priority" },
                    new TagEntity { Name = "Low Priority" });
            }

            _ = context.SaveChanges();
        }
    }
}
