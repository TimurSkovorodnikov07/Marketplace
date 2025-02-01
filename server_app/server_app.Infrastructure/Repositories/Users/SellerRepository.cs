using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server_app.Application.Abstractions.Hashing;
using server_app.Application.Repositories;
using server_app.Domain.Entities.Users.Seller;
using server_app.Domain.Model.Dtos;

namespace server_app.Infrastructure.Repositories.Users;

public class SellerRepository(
    ILogger<SellerRepository> logger,
    MainDbContext dbContext,
    IHashVerify hashVerify)
    : ISellerRepository
{
    public async Task Add(SellerEntity newSeller)
    {
        await dbContext.Sellers.AddAsync(newSeller);
        await dbContext.SaveChangesAsync();
    }


    public async Task<SellerEntity?> Get(Guid guid)
    {
        return await dbContext.Sellers
            .Include(x => x.ProductsCategories).ThenInclude(c => c.PurchasedProducts)
            .FirstOrDefaultAsync(c => c.Id == guid);
    }


    //Confirmed - подвердил почту
    //Existed - созданный, не обез что подверж
    public async Task<SellerEntity?> GetConfirmedUser(string email)
    {
        return await dbContext.Sellers
            .Include(x => x.ProductsCategories).ThenInclude(c => c.PurchasedProducts)
            .FirstOrDefaultAsync(s => s.EmailVerify == true && s.Email == email);
    }

    public async Task<SellerEntity?> GetExistingUser(string email, string password)
    {
        var confirmedUser = await GetConfirmedUser(email);

        if (confirmedUser is not null)
            return confirmedUser;

        var sellers = await dbContext.Sellers
            .Include(x => x.ProductsCategories).ThenInclude(c => c.PurchasedProducts)
            .Where(c => c.Email == email).ToArrayAsync();

        var existingUser =
            sellers.FirstOrDefault(c => hashVerify.Verify(password, c.PasswordHash));

        return existingUser;
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