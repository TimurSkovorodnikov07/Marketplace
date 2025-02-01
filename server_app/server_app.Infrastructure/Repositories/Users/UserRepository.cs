using server_app.Application.Repositories;
using server_app.Domain.Entities.Users.Customer;
using server_app.Domain.Entities.Users.Seller;

namespace server_app.Infrastructure.Repositories.Users;

public class UserRepository(
    MainDbContext dbContext,
    ICustomerRepository customerRepository,
    ISellerRepository sellerRepository) : IUserRepository
{
    public async Task<(UserEntity? foundUser, bool isSeller)> Get(Guid guid)
    {
        var foundCustomer = await customerRepository.Get(guid);

        if (foundCustomer != null)
            return (foundCustomer, false);
        
        var seller = await sellerRepository.Get(guid);
        return (seller, true);
    }
    public async Task<(CustomerEntity? customer, SellerEntity? seller)>GetWithInfoWhoIt(Guid guid)
    {
        var foundCustomer = await customerRepository.Get(guid);

        if (foundCustomer != null)
            return (foundCustomer, null);
        
        var foundSeller = await sellerRepository.Get(guid);    
        return (null, foundSeller);
    }

    //Confirmed - подвердил почту
    //Existed - созданный, не обез что подверж
    public async Task<UserEntity?> GetConfirmedUser(string email)
    {
        var confirmedCustomer = await customerRepository.GetConfirmedUser(email);

        return confirmedCustomer == null
            ? await sellerRepository.GetConfirmedUser(email)
            : confirmedCustomer;
    }

    public async Task<UserEntity?> GetExistingUser(string email, string password)
    {
        var confirmedCustomer = await customerRepository.GetExistingUser(email, password);

        return confirmedCustomer == null
            ? await sellerRepository.GetExistingUser(email, password)
            : confirmedCustomer;
    }

    public async Task<bool> EmailVerifyUpdate(Guid guid)
    {
        var (foundUser, isSeller) = await Get(guid);

        if (foundUser == null)
            return false;
        
        foundUser.EmailVerify = true;
        await dbContext.SaveChangesAsync();
        
        return true;
    }
}