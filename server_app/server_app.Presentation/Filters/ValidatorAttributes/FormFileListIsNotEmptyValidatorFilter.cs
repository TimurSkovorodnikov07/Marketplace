using System.ComponentModel.DataAnnotations;

namespace server_app.Presentation.Filters.ValidatorAttributes;

public class FormFileListIsNotEmptyValidatorFilter : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is List<IFormFile> { Count: > 0 })
            return ValidationResult.Success;

        var logger = validationContext.GetService<ILogger<FormFileListIsNotEmptyValidatorFilter>>();
        if (logger != null)
        {
            logger.LogError("List is invalid");
        }
        else
        {
            var errorText = "Logger from ListIsNotEmptyValidationFilter is invalid";
            throw new NullReferenceException(errorText);
        }

        return new ValidationResult("List must not be empty.");
    }
}