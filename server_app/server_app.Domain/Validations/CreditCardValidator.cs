using FluentValidation;
using server_app.Domain.Entities.Users.CreditCard;

namespace server_app.Domain.Validations;

public class CreditCardValidator : AbstractValidator<CreditCardEntity>
{
    private CreditCardValidator()
    {
        RuleFor(x => x.NumberHash).NotEmpty();
    }

    public static bool IsValid(CreditCardEntity creditCard) => new CreditCardValidator().Validate(creditCard).IsValid;
}