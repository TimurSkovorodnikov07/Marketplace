using FluentValidation;

namespace server_app.Domain.Validations;

public abstract class UserValidator<UserType> : AbstractValidator<UserType> 
    where UserType : UserEntity
{
    protected UserValidator()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("Id is null");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(24).WithMessage("Name is empty");
        
        RuleFor(user => user.Email)
            .Must(EmailValidator.IsValid)
            .WithMessage("Invalid email address.");

        RuleFor(x => x.PasswordHash).NotEmpty().WithMessage("PasswordHash is empty");
    }
}