using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Todo.DAL;
using Todo.ManagerLibraryTests.Seed.SeedData;

namespace Todo.ManagerLibraryTests.Seed;

public class PostgresFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;
    private DataContext _context;

    public PostgresFixture()
    {
        _container = new PostgreSqlBuilder("postgres:16-alpine")
            .Build();
    }

    public DataContext GetContext() => _context;

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseNpgsql(_container.GetConnectionString())
            .Options;

        _context = new DataContext(options);
        
        await _context.Database.MigrateAsync();
        await SeedDefaultDataAsync();
    }

    private async Task SeedDefaultDataAsync()
    {
        await SeedUsers.Seed(_context);
        await SeedTodoItems.Seed(_context);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await _container.StopAsync();
    }
}