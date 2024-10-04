using System.Text.RegularExpressions;
using FluentValidation;

public class CreditCardValidator : AbstractValidator<CreditCardEntity>
{
    private CreditCardValidator()
    {
        RuleFor(x => x.Number).MaximumLength(20).NotEmpty();
        RuleFor(x => x.OwnerId).NotEmpty();
        RuleFor(x => x).Must(CardValidationCheck);
    }

    public static bool IsValid(CreditCardEntity creditCard) => new CreditCardValidator().Validate(creditCard).IsValid;

    private bool CardValidationCheck(CreditCardEntity creditCard)
    {
        var dictionary = new Dictionary<CreditCardType, string>()
        {
            { CreditCardType.MasterCard, "MasterCard" },
            { CreditCardType.VisaCard, "MasterCard" },
        };
        var pattern = dictionary.GetValueOrDefault(creditCard.Type);

        if (pattern is null)
            return false;

        var regex = new Regex(pattern);
        return regex.Match(creditCard.Number).Success;
    }
}