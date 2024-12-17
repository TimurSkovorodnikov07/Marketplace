using System.Text.RegularExpressions;
using FluentValidation;

public class CreditCardValidator : AbstractValidator<CreditCardEntity>
{
    private CreditCardValidator()
    {
        RuleFor(x => x.NumberHash).NotEmpty();
    }

    public static bool IsValid(CreditCardEntity creditCard) => new CreditCardValidator().Validate(creditCard).IsValid;
}