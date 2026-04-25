using Todo.DAL;
using Todo.DAL.Models.Identity;

namespace Todo.ManagerLibraryTests.Seed.SeedData;

public static class SeedUsers
{
    public static async Task Seed(DataContext context)
    {
        context.Users.AddRange(new ApplicationUser
        {
            Id = "1",
            UserName = "User1",
            Email = "User1@example.com",
            EmailConfirmed = true,
        }, new ApplicationUser
        {
            Id = "2",
            UserName = "User2",
            Email = "User2@example.com",
            EmailConfirmed = true,
        });

        await context.SaveChangesAsync();
    }
}