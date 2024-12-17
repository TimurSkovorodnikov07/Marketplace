using System.Text.RegularExpressions;
using FluentValidation;

public class CreditCardAddQueryValidator : AbstractValidator<CreditCardAddQuery>
{
    public CreditCardAddQueryValidator()
    {
        RuleFor(x => x.Number).NotEmpty();
        RuleFor(x => x.Many).NotEmpty();
        RuleFor(x => x.Type).NotEmpty();
        RuleFor(x => x).Must(CardNumberValidationCheck);
    }

    public static bool IsValid(CreditCardAddQuery creditCard) =>
        new CreditCardAddQueryValidator().Validate(creditCard).IsValid;

    private static bool CardNumberValidationCheck(CreditCardAddQuery creditCard)
    {
        if (!Enum.TryParse(creditCard.Type, true, out CreditCardType cardType))
            return false;

        //https://stackoverflow.com/questions/9315647/regex-credit-card-number-tests
        var dictionary = new Dictionary<CreditCardType, string>()
        {
            {
                CreditCardType.VisaCard,
                "^4[0-9]{12}(?:[0-9]{3})?$"
            },
            {
                CreditCardType.MasterCard,
                "^(5[1-5][0-9]{14}|2(22[1-9][0-9]{12}|2[3-9][0-9]{13}|[3-6][0-9]{14}|7[0-1][0-9]{13}|720[0-9]{12}))$"
            },
        };
        var pattern = dictionary.GetValueOrDefault(cardType);

        if (pattern is null)
            return false;

        var regex = new Regex(pattern);
        return regex.Match(creditCard.Number).Success;
    }
}