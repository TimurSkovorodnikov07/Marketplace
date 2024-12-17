using System.Text.RegularExpressions;
using FluentValidation;

public class ImageValidator : AbstractValidator<ImageEntity>
{
    public static readonly List<string> imageAllowedTypes = new() { "image/jpeg", "image/jpg", "image/png" };
    private const string _fileRegexPattern = "^(.+)\\/([^\\/]+)$\n";

    private ImageValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FileName).NotEmpty().NotNull();
        RuleFor(x => x.Path).NotEmpty().NotNull();
        RuleFor(x => x.MimeType).Must(x =>
        {
            foreach (var allowedType in imageAllowedTypes)
            {
                if (x == allowedType)
                    return true;
            }

            return false;
        }).NotEmpty().NotNull();
    }

    public static bool IsValid(ImageEntity imageEntity) => new ImageValidator().Validate(imageEntity).IsValid;
}