using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.PostgresTypes;
using Testcontainers.PostgreSql;
using Todo.DAL;
using Todo.DAL.DTO.TodoItem;
using Todo.DAL.Models;
using Todo.ManagerLibrary.Managers;
using Todo.ManagerLibrary.Managers.Interface;
using Todo.ManagerLibraryTests.Seed;
using Todo.ManagerLibraryTests.Setup;

namespace Todo.ManagerLibraryTests.Tests.TodoItemTests;

public class TodoItemTests : IAsyncLifetime
{
    private IServiceProvider _serviceProvider;
    private DataContext _context;
    private TodoItemManager _todoItemManager;
    private PostgresFixture _fixture;

    public async ValueTask InitializeAsync()
    {
        _fixture = new PostgresFixture();
        await _fixture.InitializeAsync();
        _context = _fixture.GetContext();
        _todoItemManager = new TodoItemManager(_context);
    }

    public async ValueTask DisposeAsync()
    {
        await _fixture.DisposeAsync();
    }
    
    [Fact]
    public async Task GetTodoItems_Valid_ShouldReturnUncompletedTodoItems()
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == "1");
        var results = await _todoItemManager.GetTodoItems(user, 1, 10);
        
        Assert.NotEmpty(results);
        Assert.Single(results);
    }
    
    [Fact]
    public async Task GetTodoItems_Valid_ShouldReturnCompletedTodoItems()
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == "1");
        var results = await _todoItemManager.GetCompletedTodoItems(user, 1, 10);
        
        Assert.NotEmpty(results);
        Assert.Single(results);
    }
    
    [Fact]
    public async Task GetTodoItem_Valid_ShouldReturnTodoItem()
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == "1");
        var result = await _todoItemManager.GetTodoItem(user, 1);
        
        Assert.NotNull(result);
        Assert.Equal(1, result?.Id);
    }
    
    [Fact]
    public async Task CreateTodoItem_ValidTodoItem_ShouldReturnTodoItem()
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == "1");
        var newTodoItem = new CreateTodoItem
        {
            UserId = user.Id,
            Title = "New Todo Task",
            Description = "New task description",
            DueDate = new DateTime(2100, 1, 1, 12, 0, 0, DateTimeKind.Utc)
        };
        var result = await _todoItemManager.CreateTodoItem(user, newTodoItem);
        
        Assert.NotNull(result);
        Assert.Equal(newTodoItem.Title, result?.Title);
        Assert.Equal(newTodoItem.Description, result?.Description);
        Assert.Equal(newTodoItem.DueDate, result?.DueDate);
    }
    
    [Fact]
    public async Task CreateTodoItem_InValidTodoItem_ShouldReturnTodoItem()
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == "1");
        var newTodoItem = new CreateTodoItem
        {
            UserId = user.Id,
            Title = "",
            Description = "New task description",
            DueDate = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc)
        };
        
        await Assert.ThrowsAsync<ValidationException>(() => 
            _todoItemManager.CreateTodoItem(user, newTodoItem));
    }
    
    [Fact]
    public async Task UpdateTodoItem_ValidTodoItem_ShouldReturnTodoItem()
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == "1");
        var existingItem = await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == 1);
        var newTodoItem = new UpdateTodoItem
        {
            Id = existingItem.Id,
            Title = "Updated Todo Task",
            Description = "Updated task description",
            DueDate = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc)
        };
        var result = await _todoItemManager.UpdateTodoItem(user, newTodoItem);
        
        Assert.NotNull(result);
        Assert.Equal(newTodoItem.Title, result?.Title);
        Assert.Equal(newTodoItem.Description, result?.Description);
        Assert.Equal(newTodoItem.DueDate, result?.DueDate);
    }
    
    [Fact]
    public async Task UpdateTodoItem_InValidTodoItem_ShouldReturnTodoItem()
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == "1");
        var newTodoItem = new UpdateTodoItem
        {
            Id = int.MaxValue,
            Title = "Updated Todo Task",
            Description = "Updated task description",
            DueDate = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc)
        };
        var result = await _todoItemManager.UpdateTodoItem(user, newTodoItem);

        Assert.Null(result);
    }
    
    [Fact]
    public async Task DeleteTodoItem_ValidId_ShouldReturnTrue()
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == "1");
        var existingItem = await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == 1);
        var result = await _todoItemManager.DeleteTodoItem(user, existingItem.Id);
        
        Assert.True(result);
    }
    
    [Fact]
    public async Task DeleteTodoItem_InvalidId_ShouldReturnFalse()
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == "1");
        var result = await _todoItemManager.DeleteTodoItem(user, int.MaxValue);
        
        Assert.False(result);
    }
}