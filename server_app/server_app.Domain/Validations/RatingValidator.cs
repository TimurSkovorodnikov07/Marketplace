using FluentValidation;
using server_app.Domain.Entities.ProductCategories.Ratings;

namespace server_app.Domain.Validations;

public class RatingValidator : AbstractValidator<RatingEntity>
{
    public RatingValidator()
    {
        RuleFor(x => x.ProductCategoryId).NotEmpty().NotNull();
    }

    public static bool IsValid(RatingEntity rating) => new RatingValidator().Validate(rating).IsValid;
}