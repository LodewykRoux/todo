using Todo.Api.Helper;
using Todo.DAL.DTO.TodoItem;
using Todo.ManagerLibrary.Managers;
using Todo.ManagerLibrary.Managers.Interface;

namespace Todo.Api.Endpoints;

public static class TodoItemEndpoints
{
    public static void RegisterTodoItemEndpoints(this IEndpointRouteBuilder app)
    {
        var todoItems = app.MapGroup("/todoitems").RequireAuthorization().WithTags("Todo Items");

        todoItems.MapGet("/",
            async (HttpContextHelper contextHelper, ITodoItemManager todoItemManager, int pageNumber = 1, int pageSize = 10) =>
            {
                var user = contextHelper.GetUser();
                if (user is null) return Results.BadRequest();

                return Results.Ok(await todoItemManager.GetTodoItems(user, pageNumber, pageSize));
            });

        todoItems.MapGet("/completed",
            async (HttpContextHelper contextHelper, ITodoItemManager todoItemManager, int pageNumber = 1, int pageSize = 10) =>
            {
                var user = contextHelper.GetUser();
                if (user is null) return Results.BadRequest();

                var items = await todoItemManager.GetCompletedTodoItems(user, pageNumber, pageSize);
                return Results.Ok(items);
            });

        todoItems.MapGet("/{id:int}",
            async (HttpContextHelper contextHelper, ITodoItemManager todoItemManager, int id) =>
            {
                var user = contextHelper.GetUser();
                if (user is null) return Results.BadRequest();

                var item = await todoItemManager.GetTodoItem(user, id);
                return item is null ? Results.NotFound() : Results.Ok(item);
            });

        todoItems.MapPost("/",
           async (HttpContextHelper contextHelper, ITodoItemManager todoItemManager, CreateTodoItem todoItem) =>
            {
                var user = contextHelper.GetUser();
                if (user is null) return Results.BadRequest();

                var item = await todoItemManager.CreateTodoItem(user, todoItem);
                return Results.Created($"/todoitems/{item.Id}", item);
            });

        todoItems.MapPut("/{id:int}",
           async (HttpContextHelper contextHelper, ITodoItemManager todoItemManager, int id, UpdateTodoItem todoItem) =>
            {
                var user = contextHelper.GetUser();
                if (user is null) return Results.BadRequest();

                todoItem.Id = id;
                var updated = await todoItemManager.UpdateTodoItem(user, todoItem);

                return updated is null ? Results.NotFound() : Results.Ok(updated);
            });

        todoItems.MapDelete("/{id:int}",
            async (HttpContextHelper contextHelper, ITodoItemManager todoItemManager, int id) =>
            {
                var user = contextHelper.GetUser();
                if (user is null) return Results.BadRequest();

                return await todoItemManager.DeleteTodoItem(user, id)
                    ? Results.NoContent()
                    : Results.NotFound();
            });

        todoItems.MapPut("/{id:int}/complete",
            async (HttpContextHelper contextHelper, ITodoItemManager todoItemManager, int id) =>
            {
                var user = contextHelper.GetUser();
                if (user is null) return Results.BadRequest();

                var item = await todoItemManager.CompleteTodoItem(user, id);
                return item is null ? Results.NotFound() : Results.Ok(item);
            });
    }
}