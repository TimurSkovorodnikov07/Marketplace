using FluentValidation;

public class ImageValidator : AbstractValidator<ImageEntity>
{
    private ImageValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Path).Must(Directory.Exists);
    }
    
    public static bool IsValid(ImageEntity imageEntity)
    {
        return new ImageValidator().Validate(imageEntity).IsValid;
    }
}