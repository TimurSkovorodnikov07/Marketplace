using FluentValidation;

public class RatingFromCustomerValidator : AbstractValidator<RatingFromCustomerEntity>
{
    public RatingFromCustomerValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().NotNull();
        RuleFor(x => x.CommonRating).NotEmpty().NotNull();
    }

    public static bool IsValid(RatingFromCustomerEntity rating) => new RatingFromCustomerValidator().Validate(rating).IsValid;
}