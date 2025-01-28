using FluentValidation;
using server_app.Domain.Entities.ProductCategories.Images;

namespace server_app.Domain.Validations;

public class ImageMongoValidator : AbstractValidator<ImageMongoEntity>
{
    public static readonly List<string> ImageAllowedMimeTypes = new() { "image/jpeg", "image/jpg", "image/png" };
    private const string FileRegexPattern = "^(.+)\\/([^\\/]+)$\n";

    private ImageMongoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.GuidId).NotEmpty();
        RuleFor(x => x.ImageData).NotEmpty().NotNull().Must(x => x.Length > 0);
        RuleFor(x => x.MimeType).Must(ImageValidator.IsMimeTypeAllowed).NotEmpty().NotNull();
    }

    public static bool IsValid(ImageMongoEntity imageMongoEntity) => new ImageMongoValidator().Validate(imageMongoEntity).IsValid;
}