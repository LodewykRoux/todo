using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Testcontainers.PostgreSql;
using Todo.DAL;

namespace Todo.ApiIntegrationTests.Setup;

public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private PostgreSqlContainer? _dbContainer;

    public async ValueTask InitializeAsync()
    {
        _dbContainer = new PostgreSqlBuilder("tododb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        if (_dbContainer != null)
        {
            await _dbContainer.StopAsync();
        }
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = _dbContainer!.GetConnectionString();

        builder.ConfigureServices(services =>
        {
            // Remove Aspire-managed DbContext
            var descriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<DataContext>) ||
                            d.ServiceType.Name.Contains("DbContext"))
                .ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            // Add test database context
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
        });

        builder.UseEnvironment("Testing");
    }
}