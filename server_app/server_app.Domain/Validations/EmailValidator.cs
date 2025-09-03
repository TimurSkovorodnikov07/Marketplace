using FluentValidation;

namespace server_app.Domain.Validations;

public class EmailValidator : AbstractValidator<string>
{
    private EmailValidator()
    {
        RuleFor(x => x).MaximumLength(45).NotEmpty()
            .EmailAddress().WithMessage("Email not valid");
    }
    
    public static bool IsValid(string email) => new EmailValidator().Validate(email).IsValid;
}