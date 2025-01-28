using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using server_app.Domain.Model.Queries;
using server_app.Domain.Validations;

namespace server_app.Infrastructure.ValidatorAttributes;

public class CreditCardAddQueryValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var logger =
            validationContext.GetRequiredService(typeof(ILogger<CreditCardAddQueryValidationAttribute>)) as
                ILogger<CreditCardAddQueryValidationAttribute>;

        if (value is not CreditCardAddQuery query)
            return new ValidationResult("This value is not a CreditCardAddQuery");

        if (CreditCardAddQueryValidator.IsValid(query))
            return ValidationResult.Success;

        logger.LogTrace("CreditCardAddQueryValidator result: Invalid query");
        return new ValidationResult("Invalid");
    }
}