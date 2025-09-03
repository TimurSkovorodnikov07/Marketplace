using server_app.Domain.Entities.Users.Customer;

namespace server_app.Domain.Validations;

public class CustomerValidator : UserValidator<CustomerEntity>
{
    private CustomerValidator()
    {
    }

    public static bool IsValid(CustomerEntity customer) => new CustomerValidator().Validate(customer).IsValid;
}