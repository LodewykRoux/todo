using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Todo.DAL;
using Todo.DAL.Models.Identity;
using Todo.ManagerLibrary.Managers;
using Todo.ManagerLibrary.Managers.Interface;
using Microsoft.AspNetCore.Identity;

namespace Todo.ManagerLibraryTests.Setup;

public class TestStartup
{
    public static async Task<(IServiceProvider provider, PostgreSqlContainer container, DataContext context)> BuildServiceProviderWithContainerAsync()
    {
        var container = new PostgreSqlBuilder("postgres:16-alpine")
            .Build();
        await container.StartAsync();

        var services = new ServiceCollection();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseNpgsql(container.GetConnectionString())
            .Options;

        services.AddScoped(_ => new DataContext(options));

        // Register Identity with the same DbContext
        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<DataContext>() ;

        services.AddScoped<IAuthManager, AuthManager>();

        var serviceProvider = services.BuildServiceProvider();

        // Create database schema
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            await context.Database.MigrateAsync();
        }

        var dbContext = serviceProvider.GetRequiredService<DataContext>();
        return (serviceProvider, container, dbContext);
    }
}