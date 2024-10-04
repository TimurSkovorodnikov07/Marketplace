using FluentValidation;

public class DeliveryCompanyValidator : AbstractValidator<DeliveryCompanyEntity>
{
    private DeliveryCompanyValidator()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("Id is null");
        RuleFor(x => x.Name).MaximumLength(24).NotEmpty().WithMessage("Name is empty");
        RuleFor(x => x.Description).MaximumLength(500).NotEmpty().WithMessage("Description is empty");
        RuleFor(x => x.WebSite).NotEmpty().WithMessage("Web site is empty or not valid");
        RuleFor(x => x.PhoneNumber).NotEmpty().Must(PhoneNumberValidator.IsValid);
    }
    
    public static bool IsValid(DeliveryCompanyEntity deliveryCompany)
    {
        return new DeliveryCompanyValidator().Validate(deliveryCompany).IsValid;
    }
}