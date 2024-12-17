using Microsoft.EntityFrameworkCore;

public class UserEntityService(
    MainDbContext dbContext,
    CustomerService customerService,
    SellerService sellerService)
{
    public async Task<(UserEntity? foundUser, bool isSeller)> Get(Guid guid)
    {
        var foundCustomer = await customerService.Get(guid);

        if (foundCustomer != null)
            return (foundCustomer, false);
        
        var seller = await sellerService.Get(guid);
        return (seller, true);
    }
    public async Task<(CustomerEntity? customer, SellerEntity? seller)>GetWithInfoWhoIt(Guid guid)
    {
        var foundCustomer = await customerService.Get(guid);

        if (foundCustomer != null)
            return (foundCustomer, null);
        
        var foundSeller = await sellerService.Get(guid);    
        return (null, foundSeller);
    }

    //Confirmed - подвердил почту
    //Existed - созданный, не обез что подверж
    public async Task<UserEntity?> GetConfirmedUser(string email)
    {
        var confirmedCustomer = await customerService.GetConfirmedUser(email);

        return confirmedCustomer == null
            ? await sellerService.GetConfirmedUser(email)
            : confirmedCustomer;
    }

    public async Task<UserEntity?> GetExistingUser(string email, string password)
    {
        var confirmedCustomer = await customerService.GetExistingUser(email, password);

        return confirmedCustomer == null
            ? await sellerService.GetExistingUser(email, password)
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