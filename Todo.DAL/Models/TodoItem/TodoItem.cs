using System.ComponentModel.DataAnnotations;
using Todo.DAL.Models.Base;

namespace Todo.DAL.Models;

public class TodoItem : BaseClass
{
    [Key]
    public int Id { get; set; }
    
    [StringLength(30)]
    public string Title { get; set; }
    
    [StringLength(100)]
    public string Description { get; set; }
    
    public DateTime? DueDate { get; set; }

    public bool IsCompleted { get; set; } = false;
}