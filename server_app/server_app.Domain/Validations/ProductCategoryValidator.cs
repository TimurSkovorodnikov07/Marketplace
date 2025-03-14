using FluentValidation;
using server_app.Domain.Entities.ProductCategories;

namespace server_app.Domain.Validations;

public class ProductCategoryValidator : AbstractValidator<ProductCategoryEntity>
{
    private ProductCategoryValidator()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("Id is null");
        RuleFor(x => x.Name).MaximumLength(24).NotNull().NotEmpty().WithMessage("Name is empty");
        RuleFor(x => x.Price).Must(p => p > 0).WithMessage("The price must be greater than 0");
        RuleFor(x => x.Quantity).Must(q => q > 0).WithMessage("The quentity must be greater than 0");

        RuleFor(x => x.Description).Must(des => string.IsNullOrEmpty(des) || des.Length <= 500)
            .WithMessage("Description not valid");

        RuleFor(x => x.Tags).NotNull().Must(tags => tags is not null && tags.Tags.Count > 0).WithMessage("Tags empty");
        RuleFor(x => x.Owner).NotNull().WithMessage("Owner is null");
        RuleFor(x => x.DeliveryCompany).NotNull().WithMessage("Delivery company is null");
    }

    public static bool IsValid(ProductCategoryEntity category) =>
        new ProductCategoryValidator().Validate(category).IsValid;
}