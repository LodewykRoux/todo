namespace Todo.DAL.Models.Base;

public class BaseClass
{
    public string UserId { get; set; } = default!;
    public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    public required string CreatedBy { get; set; }
    public required string CreatedById { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
}