using System.ComponentModel.DataAnnotations;
using Todo.DAL.ValidationAttributes;

namespace Todo.DAL.DTO.TodoItem;

public class CreateTodoItem
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 30 characters")]
    public string Title { get; set; } = default!;

    [StringLength(100, ErrorMessage = "Description must not exceed 100 characters")]
    public string? Description { get; set; }

    [FutureDate(ErrorMessage = "Due date must be in the future")]
    public DateTime? DueDate { get; set; }

    [Required(ErrorMessage = "UserId is required")]
    public string UserId { get; set; } = default!;
}