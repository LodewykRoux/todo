using System.ComponentModel.DataAnnotations;

namespace Todo.DAL.ValidationAttributes;

/// <summary>
/// Validates that a DateTime value is in the future
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is not DateTime dateTime)
        {
            return new ValidationResult("Due date must be a valid date");
        }

        if (dateTime > DateTime.UtcNow)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(
            ErrorMessage ?? "Due date must be in the future"
        );
    }
}