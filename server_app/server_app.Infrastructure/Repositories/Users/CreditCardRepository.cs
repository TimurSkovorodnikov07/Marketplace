using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server_app.Application.Repositories;
using server_app.Domain.Entities.Users.CreditCard;

namespace server_app.Infrastructure.Repositories.Users;

public class CreditCardRepository(
    MainDbContext context,
    ICustomerRepository customerRepository,
    ILogger<CreditCardRepository> logger) : ICreditCardRepository
{
    public async Task<CreditCardEntity?> Get(Guid guid) =>
        await context.CreditCards
            .Include(c => c.Owner)
            .FirstOrDefaultAsync(c => c.Id == guid);

    public async Task<bool> IsEnoughMoneyAndWriteOff(Guid guid, decimal sum)
    {
        var foundCard = await Get(guid);
        
        if (foundCard is null || foundCard.Many < sum)
            return false;
        
        foundCard.Many -= sum;
        await context.SaveChangesAsync();
        return true;
    }
}