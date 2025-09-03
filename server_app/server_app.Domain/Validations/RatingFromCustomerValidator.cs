using FluentValidation;
using server_app.Domain.Entities.ProductCategories.Ratings;

namespace server_app.Domain.Validations;

public class RatingFromCustomerValidator : AbstractValidator<RatingFromCustomerEntity>
{
    public RatingFromCustomerValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().NotNull();
        RuleFor(x => x.CommonRating).NotEmpty().NotNull();
    }

    public static bool IsValid(RatingFromCustomerEntity rating) => new RatingFromCustomerValidator().Validate(rating).IsValid;
}