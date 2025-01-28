using FluentValidation;
using server_app.Domain.Entities.Users.Seller;

namespace server_app.Domain.Validations;

public class SellerValidator : UserValidator<SellerEntity>
{
    private SellerValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    }
    public static bool IsValid(SellerEntity seller) => new SellerValidator().Validate(seller).IsValid;
}