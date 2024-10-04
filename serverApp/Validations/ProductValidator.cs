using System.Data;
using FluentValidation;

public class ProductValidator : AbstractValidator<PurchasedProductEntity>
{
    private ProductValidator()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("Id is null");
        RuleFor(x => x.Category).NotNull().WithMessage("Category is null");
        RuleFor(x => x.MustDeliveredBefore).Must(date => date > DateTime.UtcNow);
    }
    public static bool IsValid(PurchasedProductEntity purchasedProduct) => new ProductValidator().Validate(purchasedProduct).IsValid;
}
