using FluentValidation;

public class RatingValidator : AbstractValidator<RatingEntity>
{
    public RatingValidator()
    {
        RuleFor(x => x.ProductCategoryId).NotEmpty().NotNull();
    }

    public static bool IsValid(RatingEntity rating) => new RatingValidator().Validate(rating).IsValid;
}