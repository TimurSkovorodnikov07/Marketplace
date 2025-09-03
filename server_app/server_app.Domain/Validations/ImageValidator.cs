using FluentValidation;
using server_app.Domain.Entities.ProductCategories.Images;

namespace server_app.Domain.Validations;

public class ImageValidator : AbstractValidator<ImageEntity>
{
    public static readonly List<string> ImageAllowedMimeTypes = new() { "image/jpeg", "image/jpg", "image/png" };
    private const string FileRegexPattern = "^(.+)\\/([^\\/]+)$\n";

    private ImageValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.ProductCategoryId).NotEmpty();
        RuleFor(x => x.ImageData).NotEmpty().NotNull()
            .Must(x => x.Length > 0);
        RuleFor(x => x.MimeType).Must(IsMimeTypeAllowed)
            .NotEmpty().NotNull();
    }

    public static bool IsValid(ImageEntity imageEntity) =>
        new ImageValidator().Validate(imageEntity).IsValid;

    public static bool IsMimeTypeAllowed(string mimeType) =>
        ImageAllowedMimeTypes.Any(allowedType => mimeType == allowedType);
}