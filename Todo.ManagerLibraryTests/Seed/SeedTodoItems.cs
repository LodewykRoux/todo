using Todo.DAL;
using Todo.DAL.Models;

namespace Todo.ManagerLibraryTests.Seed.SeedData;

public static class SeedTodoItems
{
    public static async Task Seed(DataContext context)
    {
        context.TodoItems.AddRange(
            new TodoItem
            {
                UserId = "1",
                Title = "Complete unit tests",
                Description = "Write comprehensive unit tests",
                DueDate = DateTime.UtcNow.AddDays(7),
                IsCompleted = false,
                CreatedBy = "User1",
                CreatedById = "1"
            },
            new TodoItem
            {
                UserId = "1",
                Title = "Deploy to production",
                Description = "Push changes to production environment",
                DueDate = DateTime.UtcNow.AddDays(14),
                IsCompleted = true,
                CreatedBy = "User1",
                CreatedById = "1"
            }
        );

        await context.SaveChangesAsync();
    }
}