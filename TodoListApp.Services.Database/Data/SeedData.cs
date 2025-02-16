using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoListApp.WebApi.Models.Entities;

namespace TodoListApp.Services.Database.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            if (!context.Tag.Any())
            {
                context.Tag.AddRange(
                    new TagEntity { Name = "High Priority" },
                    new TagEntity { Name = "Normal Priority" },
                    new TagEntity { Name = "Low Priority" });
                _ = context.SaveChanges();
            }
        }
    }
}
