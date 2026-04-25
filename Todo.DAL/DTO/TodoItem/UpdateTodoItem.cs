using System.ComponentModel.DataAnnotations;

namespace Todo.DAL.DTO.TodoItem;

public class UpdateTodoItem
{
    public int Id { get; set; }
    
    [StringLength(30)]
    public string Title { get; set; }
    
    [StringLength(100)]
    public string Description { get; set; }
    
    public DateTime? DueDate { get; set; }
}