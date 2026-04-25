using System.Net;
using System.Net.Http.Json;
using Todo.ApiIntegrationTests.Setup;

namespace Todo.ApiIntegrationTests.Tests.TodoItemTests;

public class TodoItemTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateTodoItem_WithValidData_ReturnsCreated()
    {
        // Arrange
        var createRequest = new
        {
            title = "Test Todo Item",
            description = "Test Description"
        };

        // Act
        var response = await PostJsonAsync("/api/todos", createRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<dynamic>();
        Assert.NotNull(result);
    }
}