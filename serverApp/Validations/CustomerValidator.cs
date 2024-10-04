using FluentValidation;

public class CustomerValidator : UserValidator<CustomerEntity>
{
    private CustomerValidator()
    {
    }

    public static bool IsValid(CustomerEntity customer) => new CustomerValidator().Validate(customer).IsValid;
}