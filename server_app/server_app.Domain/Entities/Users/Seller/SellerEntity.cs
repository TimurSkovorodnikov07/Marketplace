using server_app.Domain.Entities.ProductCategories;
using server_app.Domain.Validations;

namespace server_app.Domain.Entities.Users.Seller;

public class SellerEntity : UserEntity
{
    public string Description { get; set; }
    public List<ProductCategoryEntity> ProductsCategories{ get; set; }

    public static SellerEntity? Create(string name, string description, string email, string passwordHash)
    {
        var seller = new SellerEntity
        {
            Name = name,
            Email = email,
            Description = description,
            PasswordHash = passwordHash,
            EmailVerify = false
        };

        return SellerValidator.IsValid(seller) ? seller : null;
    }
}