public class CustomerEntity : UserEntity
{
    public List<PurchasedProductEntity> Purchases { get; set; }
    public Guid CreditCardId { get; set; }
        
    public static CustomerEntity? Create(string name, string email, string passwordHash)
    {
        var customer = new CustomerEntity
        {
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            EmailVerify = false,
        };

        if (CustomerValidator.IsValid(customer))
            return customer;

        return null;
    }
    public static CustomerEntity? Create(CustomerRegistrationQuery dto, string passwordHash) => Create(dto.Name, dto.Email, passwordHash);
}

