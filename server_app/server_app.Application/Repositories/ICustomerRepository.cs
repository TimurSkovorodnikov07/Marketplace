using server_app.Domain.Entities.Users.CreditCard;
using server_app.Domain.Entities.Users.Customer;
using server_app.Domain.Model;
using server_app.Domain.Model.Dtos;

namespace server_app.Application.Repositories;

public interface ICustomerRepository : IUserRepository<CustomerEntity, UserUpdateDto>
{
    Task AddCard(CreditCardEntity newCreditCard, Guid ownerId);
}