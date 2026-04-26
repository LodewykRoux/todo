using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Todo.DAL;
using Todo.DAL.DTO.Identity;
using Todo.ManagerLibrary.Managers.Interface;
using Todo.ManagerLibraryTests.Setup;

namespace Todo.ManagerLibraryTests.Tests.UserTests;

public class UserTests : IAsyncLifetime
{
    private IServiceProvider _serviceProvider;
    private PostgreSqlContainer _container;
    private DataContext _context;
    private IAuthManager _authManager;

    public async ValueTask InitializeAsync()
    {
        var result = await TestStartup.BuildServiceProviderWithContainerAsync();
        _serviceProvider = result.provider;
        _container = result.container;
        _context = result.context;
        _authManager = _serviceProvider.GetRequiredService<IAuthManager>();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await _container.StopAsync();
    }

    [Fact]
    public async Task RegisterUser_ValidUser_ReturnsSuccessResult()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "TestPassword123"
        };

        // Act
        var (result, user) = await _authManager.RegisterUser(request);

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(user);
        Assert.NotEmpty(user.Id);
        Assert.Equal("test@example.com", user.Email);
    }

    [Fact]
    public async Task FindByEmail_UserExists_ReturnsUser()
    {
        // Arrange
        await _authManager.RegisterUser(new RegisterRequest
        {
            Email = "findme@example.com",
            Password = "TestPassword123"
        });

        // Act
        var user = await _authManager.FindByEmail("findme@example.com");

        // Assert
        Assert.NotNull(user);
        Assert.Equal("findme@example.com", user.Email);
    }

    [Fact]
    public async Task CheckPassword_ValidPassword_ReturnsTrue()
    {
        // Arrange
        var password = "TestPassword123";
        var (_, user) = await _authManager.RegisterUser(new RegisterRequest
        {
            Email = "password@example.com",
            Password = password
        });

        // Act
        var isValid = await _authManager.CheckPassword(user, password);

        // Assert
        Assert.True(isValid);
    }
}