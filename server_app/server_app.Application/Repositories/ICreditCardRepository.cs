using server_app.Domain.Entities.Users.CreditCard;

namespace server_app.Application.Repositories;

public interface ICreditCardRepository
{ Task<CreditCardEntity?> Get(Guid guid);
    Task<bool> IsEnoughMoneyAndWriteOff(Guid guid, decimal sum);
}