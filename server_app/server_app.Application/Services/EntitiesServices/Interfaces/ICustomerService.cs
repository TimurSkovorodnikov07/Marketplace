using server_app.Domain.Entities.Users.CreditCard;
using server_app.Domain.Entities.Users.Customer;
using server_app.Domain.Model;
using server_app.Domain.Model.Dtos;

namespace server_app.Application.Services.EntitiesServices.Interfaces;

public interface ICustomerService : IUserService<CustomerEntity, UserUpdateDto>
{
    Task<Result> AddCard(CreditCardEntity newCreditCard, Guid ownerId);
}