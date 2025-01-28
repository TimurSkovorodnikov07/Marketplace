using server_app.Domain.Entities.ProductCategories.PurchasedProducts;
using server_app.Domain.Entities.Users.CreditCard;
using server_app.Domain.Validations;

namespace server_app.Domain.Entities.Users.Customer;

public class CustomerEntity : UserEntity
{
    public List<PurchasedProductEntity> Purchases { get; set; }
    public CreditCardEntity CreditCard { get; set; }

    public static CustomerEntity? Create(string name, string email, string passwordHash)
    {
        var customer = new CustomerEntity
        {
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
        };

        return CustomerValidator.IsValid(customer) ? customer : null;
    }
}