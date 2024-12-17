using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CreditCardService(
    MainDbContext context,
    CustomerService customerService,
    ILogger<CreditCardService> logger)
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