using Todo.DAL.Models;

namespace Todo.ManagerLibrary.Managers.Interface;

public interface IAgentManager
{
    Task<List<TodoItem>> GenerateTodoItem(string prompt);
}