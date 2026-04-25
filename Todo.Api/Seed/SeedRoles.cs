using Microsoft.AspNetCore.Identity;
using Todo.DAL.Models.Identity;

namespace Todo.Api.Seed;

public static class SeedRoles
{
    public static async Task Seed(RoleManager<ApplicationRole> roleManager)
    {
        string[] roles = ["Admin", "Manager", "User"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = role,
                    Description = role
                });
            }
        }
    }
}