using System.ComponentModel.DataAnnotations;

public class CreditCardAddQueryValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var logger =
            validationContext.GetRequiredService(typeof(ILogger<CreditCardAddQueryValidationAttribute>)) as
                Logger<CreditCardAddQueryValidationAttribute>;

        if (value is not CreditCardAddQuery query)
            return new ValidationResult("This value is not a CreditCardAddQuery");

        if (CreditCardAddQueryValidator.IsValid(query))
            return ValidationResult.Success;

        logger.LogTrace("CreditCardAddQueryValidator result: Invalid query");
        return new ValidationResult("Invalid");
    }
}