using System.Data;
using Microsoft.EntityFrameworkCore;

public class SellerService(
    ILogger<CustomerService> logger,
    MainDbContext dbContext)
    : IUserService<SellerEntity, UserUpdateDto>
{
    private readonly ILogger<CustomerService> _logger = logger;

    public async Task Add(SellerEntity newSeller)
    {
        await dbContext.Sellers.AddAsync(newSeller);
        await dbContext.SaveChangesAsync();
    }


    public async Task<SellerEntity?> Get(Guid guid)
    {
        return await dbContext.Sellers
            .Include(x => x.ProductsCategories).ThenInclude(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == guid);
    }

    public async Task<SellerEntity?> GetConfirmedUser(string email)
    {
        //Confirmed - подвердил почту
        //Existed - созданный, не обез что подверж
        return await dbContext.Sellers
            .Include(x => x.ProductsCategories).ThenInclude(c => c.Products)
            .FirstOrDefaultAsync(s => s.EmailVerify == true
                                                               && s.Email == email);
    }

    public async Task<SellerEntity?> GetExistingUser(string email, string passwordHash)
    {
        var users = dbContext.Sellers
            .Include(x => x.ProductsCategories).ThenInclude(c => c.Products)
            .Where(c => c.Email == email);
        var confirmedUser = await users.FirstOrDefaultAsync(x => x.EmailVerify == true);

        if (confirmedUser is null)
            return await users.FirstOrDefaultAsync(x => x.PasswordHash == passwordHash);

        return confirmedUser;
    }


    public async Task<bool> EmailVerUpdate(Guid guid)
    {
        var updatedSellers = await Get(guid);

        if (updatedSellers == null)
            return false;

        updatedSellers.EmailVerify = true;

        dbContext.Sellers.Update(updatedSellers);
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Update(UserUpdateDto updatedSeller)
    {
        var seller = await Get(updatedSeller.Id);

        if (seller == null)
            return false;

        seller.Name = updatedSeller.NewName;
        seller.Name = updatedSeller.NewEmail;
        seller.Name = updatedSeller.NewPassword;

        dbContext.Sellers.Update(seller);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Remove(Guid guid)
    {
        var deletedSeller = await Get(guid);

        if (deletedSeller == null)
            return false;

        dbContext.Sellers.Remove(deletedSeller);
        await dbContext.SaveChangesAsync();

        return true;
    }
}