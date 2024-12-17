using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CustomerService(
    ILogger<CustomerService> logger,
    MainDbContext dbContext,
    IHashVerify hashVerify)
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
            .Include(c => c.CreditCard)
            .Where(c => c.Id == guid).FirstOrDefaultAsync();
    }



    //Confirmed - подвердил почту
    //Existed - созданный, не обез что подверж
    public async Task<CustomerEntity?> GetConfirmedUser(string email)
    {
        return await dbContext.Customers
            .Include(c => c.Purchases)
            .Include(c => c.CreditCard)
            .FirstOrDefaultAsync(s => s.EmailVerify == true
                                      && s.Email == email);
    }

    public async Task<CustomerEntity?> GetExistingUser(string email, string password)
    {
        var confirmedUser = await GetConfirmedUser(email);

        if (confirmedUser is not null)
            return confirmedUser;

        var customers = await dbContext.Customers
            .Include(c => c.Purchases)
            .Include(c => c.CreditCard)
            .Where(c => c.Email == email).ToArrayAsync();

        var existingUser =
            customers.FirstOrDefault(c => hashVerify.Verify(password, c.PasswordHash));
        
        return existingUser;
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

    public async Task<Result> AddCard(CreditCardEntity newCreditCard, Guid ownerId)
    {
        var customer = await Get(ownerId);

        if (customer == null)
            return Result.NotFound("Customer not found");

        await dbContext.CreditCards.AddAsync(newCreditCard);
        await dbContext.SaveChangesAsync();

        return Result.Ok();
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