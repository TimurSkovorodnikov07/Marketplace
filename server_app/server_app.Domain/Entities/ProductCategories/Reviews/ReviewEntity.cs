using server_app.Domain.Entities.Users.Customer;
using server_app.Domain.Validations;

namespace server_app.Domain.Entities.ProductCategories.Reviews;

public class ReviewEntity : Entity
{
    public string Text { get; set; }
    public int Estimation { get; set; }

    public ProductCategoryEntity Category;
    public Guid CategoryId;

    public CustomerEntity ReviewOwner;
    public Guid ReviewOwnerId;

    public static ReviewEntity? Create(string text, int estimation,
        ProductCategoryEntity category, CustomerEntity reviewOwner)
    {
        var newReview = new ReviewEntity
        {
            Text = text,
            Estimation = estimation,
            Category = category,
            ReviewOwner = reviewOwner
        };
        return ReviewValidator.IsValid(newReview) ? newReview : null;
    }
}