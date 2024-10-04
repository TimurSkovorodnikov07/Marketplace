using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CustomerService(
    ILogger<CustomerService> logger,
    MainDbContext dbContext)
    : IUserService<CustomerEntity, UserUpdateDto>
{
    private readonly ILogger<CustomerService> _logger = logger;

    public async Task Add(CustomerEntity newCustomer)
    {
        await dbContext.Customers.AddAsync(newCustomer);
        await dbContext.SaveChangesAsync();
    }


    public async Task<CustomerEntity?> Get(Guid guid)
    {
        return await dbContext.Customers
            .Include(c => c.Purchases)
            .Where(c => c.Id == guid).FirstOrDefaultAsync();
    }

    public async Task<CustomerEntity?> GetConfirmedUser(string email)
    {
        //Confirmed - подвердил почту
        //Existed - созданный, не обез что подверж
        return await dbContext.Customers
            .Include(c => c.Purchases)
            .FirstOrDefaultAsync(s => s.EmailVerify == true
                                      && s.Email == email);
    }

    public async Task<CustomerEntity?> GetExistingUser(string email, string passwordHash)
    {
        var users = dbContext.Customers
            .Include(c => c.Purchases)
            //.ThenInclude(p => p.Seller)
            .Where(c => c.Email == email);
        var confirmedUser = await users.FirstOrDefaultAsync(x => x.EmailVerify == true);

        if (confirmedUser is null)
            return await users.FirstOrDefaultAsync(x => x.PasswordHash == passwordHash);

        return confirmedUser;
    }


    public async Task<bool> EmailVerUpdate(Guid guid)
    {
        var updatedCustomer = await Get(guid);

        if (updatedCustomer == null)
            return false;

        updatedCustomer.EmailVerify = true;
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Update(UserUpdateDto updatedCustomer)
    {
        var customer = await Get(updatedCustomer.Id);

        if (customer == null)
            return false;

        customer.Name = updatedCustomer.NewName;
        customer.Name = updatedCustomer.NewEmail;
        customer.Name = updatedCustomer.NewPassword;

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddCard(CreditCardEntity newCreditCard)
    {
        var customer = await Get(newCreditCard.Id);

        if (customer == null)
            return false;
        
        await dbContext.CreditCards.AddAsync(newCreditCard);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Remove(Guid guid)
    {
        var deletedCustomer = await Get(guid);

        if (deletedCustomer == null)
            return false;

        dbContext.Customers.Remove(deletedCustomer);
        await dbContext.SaveChangesAsync();
        return true;
    }
}