using FluentValidation;
using server_app.Domain.Entities.ProductCategories.Reviews;

namespace server_app.Domain.Validations;

public class ReviewValidator : AbstractValidator<ReviewEntity>
{
    public ReviewValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Text).NotEmpty().MinimumLength(10).MaximumLength(500);
        RuleFor(x => x.Estimation).NotEmpty().Must(x => x >= 1 && x <= 10);
    }
    
    public static bool IsValid(ReviewEntity? review) => new ReviewValidator().Validate(review).IsValid;
}