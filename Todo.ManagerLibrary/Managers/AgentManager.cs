using Todo.AI;
using Todo.DAL.DTO.Identity;
using Todo.DAL.Models;
using Todo.ManagerLibrary.Managers.Interface;

namespace Todo.ManagerLibrary.Managers;

public class AgentManager : IAgentManager
{
    public async Task<List<TodoItem>> GenerateTodoItem(string prompt)
    {
        var result = await Generate.GenerateTodoItems(prompt);

        return [];
    }
}