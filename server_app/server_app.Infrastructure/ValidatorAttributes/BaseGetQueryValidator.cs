using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using server_app.Domain.Model.Queries;

namespace server_app.Infrastructure.ValidatorAttributes;

public class BaseGetQueryValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var logger =
            validationContext.GetRequiredService(typeof(ILogger<CreditCardAddQueryValidationAttribute>)) as
                ILogger<CreditCardAddQueryValidationAttribute>;

        if (value is not BaseGetQuery query)
            return new ValidationResult("This value is not a BaseGetQuery");

        if (query.From < query.To)
            return ValidationResult.Success;

        logger.LogTrace("BaseGetQueryValidator result: Invalid query");
        return new ValidationResult("Invalid");
    }   
}