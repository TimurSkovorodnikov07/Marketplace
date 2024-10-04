using Microsoft.EntityFrameworkCore;

public class CreditCardService(
    MainDbContext context,
    CustomerService customerService,
    ILogger<CreditCardService> logger)
{
    public async Task<CreditCardEntity?> Get(Guid guid) =>
        await context.CreditCards.FirstOrDefaultAsync(c => c.Id == guid);

    public async Task<bool> IsEnoughManyAndWriteOff(Guid id, decimal sum)
    {
        var foundCard = await Get(id);

        if (foundCard == null || foundCard.Many < sum)
            return false;

        foundCard.Many -= sum;

        await context.SaveChangesAsync();
        return true;
    }
}