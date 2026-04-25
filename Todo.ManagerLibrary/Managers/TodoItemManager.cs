using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Todo.DAL;
using Todo.DAL.DTO;
using Todo.DAL.DTO.TodoItem;
using Todo.DAL.Models;
using Todo.DAL.Models.Identity;
using Todo.ManagerLibrary.Managers.Interface;
using X.PagedList;

namespace Todo.ManagerLibrary.Managers;

public class TodoItemManager(DataContext dataContext) : ITodoItemManager
{
    public async Task<IPagedList<TodoItem>> GetTodoItems(ApplicationUser user, int pageNumber, int pageSize)
    {
        var todoItems = dataContext.TodoItems
            .Where(x => x.UserId == user.Id)
            .Where(x => !x.IsCompleted);

        return await todoItems.ToPagedListAsync(pageNumber, pageSize);
    }

    public async Task<IPagedList<TodoItem>> GetCompletedTodoItems(ApplicationUser user, int pageNumber, int pageSize)
    {
        var todoItems = dataContext.TodoItems
            .Where(x => x.UserId == user.Id)
            .Where(x => x.IsCompleted);

        return await todoItems.ToPagedListAsync(pageNumber, pageSize);
    }

    public async Task<TodoItem?> GetTodoItem(ApplicationUser user, int id)
    {
        var todoItem = await dataContext.TodoItems
            .Where(x => x.UserId == user.Id)
            .FirstOrDefaultAsync(x => x.Id == id);

        return todoItem;
    }

    public async Task<TodoItem?> CreateTodoItem(ApplicationUser user, CreateTodoItem todoItem)
    {
        var (isValid, errors) = ValidationHelper.ValidateObject(todoItem);
        if (!isValid)
        {
            throw new ValidationException(string.Join("; ", errors));
        }
        
        var newTodoItem = new TodoItem
        {
            CreatedBy = user.UserName ?? "System",
            CreatedById = user.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            DueDate = todoItem.DueDate,
            IsCompleted = false,
            UserId = user.Id
        };

        dataContext.TodoItems.Add(newTodoItem);
        await dataContext.SaveChangesAsync();

        return newTodoItem;
    }

    public async Task<TodoItem?> UpdateTodoItem(ApplicationUser user, UpdateTodoItem todoItem)
    {
        var existingItem = dataContext.TodoItems
            .Where(x => x.UserId == user.Id)
            .FirstOrDefault(x => x.Id == todoItem.Id);
        if (existingItem == null) return null;

        existingItem.Title = todoItem.Title;
        existingItem.Description = todoItem.Description;
        existingItem.DueDate = todoItem.DueDate;

        await dataContext.SaveChangesAsync();

        return existingItem;
    }

    public async Task<bool> DeleteTodoItem(ApplicationUser user, int id)
    {
        var existingItem = dataContext.TodoItems
            .Where(x => x.UserId == user.Id)
            .FirstOrDefault(x => x.Id == id);
        if (existingItem == null) return false;

        existingItem.DeletedBy = "System";
        existingItem.DeletedOn = DateTime.UtcNow;
        await dataContext.SaveChangesAsync();

        return true;
    }

    public async Task<TodoItem?> CompleteTodoItem(ApplicationUser user, int id)
    {
        var todoItem = dataContext.TodoItems
            .Where(x => x.UserId == user.Id)
            .FirstOrDefault(x => x.Id == id);
        if (todoItem == null) return null;

        todoItem.IsCompleted = true;
        todoItem.UpdatedOn = DateTime.UtcNow;
        await dataContext.SaveChangesAsync();

        return todoItem;
    }
}