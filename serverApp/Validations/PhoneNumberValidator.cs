using System.Text.RegularExpressions;
using FluentValidation;

public class PhoneNumberValidator : AbstractValidator<PhoneNumberValueObject>
{
    private PhoneNumberValidator()
    {
        RuleFor(p => p.Number)
       .NotEmpty()
       .NotNull().WithMessage("Phone Number is required.")
       .MinimumLength(10).WithMessage("PhoneNumber must not be less than 10 characters.")
       .MaximumLength(20).WithMessage("PhoneNumber must not exceed 20 characters.")
       .Matches(new Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"))
       .WithMessage("PhoneNumber not valid");
    }
    
    public static bool IsValid(PhoneNumberValueObject phoneNumber)
    {
        return new PhoneNumberValidator().Validate(phoneNumber).IsValid;
    }
}