using FluentValidation;

public class SellerValidator : UserValidator<SellerEntity>
{
    private SellerValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    }
    public static bool IsValid(SellerEntity seller) => new SellerValidator().Validate(seller).IsValid;
}