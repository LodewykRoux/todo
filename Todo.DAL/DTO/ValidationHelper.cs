using System.ComponentModel.DataAnnotations;

namespace Todo.DAL.DTO;

public static class ValidationHelper
{
    public static (bool isValid, List<string> errors) ValidateObject<T>(T obj) where T : class
    {
        var context = new ValidationContext(obj, serviceProvider: null, items: null);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(obj, context, results, validateAllProperties: true);

        var errors = results.Select(r => r.ErrorMessage ?? "Unknown error").ToList();

        return (isValid, errors);
    }
}