using Todo.DAL.DTO.TodoItem;
using Todo.DAL.Models;
using Todo.DAL.Models.Identity;
using X.PagedList;

namespace Todo.ManagerLibrary.Managers.Interface;

public interface ITodoItemManager
{
    Task<IPagedList<TodoItem>> GetTodoItems(ApplicationUser user, int pageNumber, int pageSize);
    Task<IPagedList<TodoItem>> GetCompletedTodoItems(ApplicationUser user, int pageNumber, int pageSize);
    Task<TodoItem?> GetTodoItem(ApplicationUser user, int id);
    Task<TodoItem?> CreateTodoItem(ApplicationUser user, CreateTodoItem todoItem);
    Task<TodoItem?> UpdateTodoItem(ApplicationUser user, UpdateTodoItem todoItem);
    Task<bool> DeleteTodoItem(ApplicationUser user, int id);
    Task<TodoItem?> CompleteTodoItem(ApplicationUser user, int id);
}