using System.ComponentModel.DataAnnotations;

namespace server_app.Presentation.Filters.ValidatorAttributes;

public class ListIsNotEmptyValidatorFilter : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        return value is List<object> { Count: > 0 }
            ? ValidationResult.Success
            : new ValidationResult("Invalid");
    }
}