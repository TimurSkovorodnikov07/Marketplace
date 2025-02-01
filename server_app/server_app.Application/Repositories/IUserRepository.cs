using server_app.Domain.Entities.Users.Customer;
using server_app.Domain.Entities.Users.Seller;

namespace server_app.Application.Repositories;

public interface IUserRepository<UserT, UpdatedEntityT> : IEntityRepository<UserT, UpdatedEntityT>
    where UserT : UserEntity
    where UpdatedEntityT : IUserUpdate
{
    Task<UserT?> Get(Guid guid);
    Task<UserT?> GetConfirmedUser(string email);
    Task<UserT?> GetExistingUser(string email, string password);

    Task Add(UserT newUser);
    Task<bool> Update(UpdatedEntityT updatedUser);
    Task<bool> Remove(Guid guid);
    Task<bool> EmailVerUpdate(Guid guid);
}

public interface IUserRepository
{
    Task<(UserEntity? foundUser, bool isSeller)> Get(Guid guid);
    Task<(CustomerEntity? customer, SellerEntity? seller)> GetWithInfoWhoIt(Guid guid);
    Task<UserEntity?> GetConfirmedUser(string email);
    Task<UserEntity?> GetExistingUser(string email, string password);
    Task<bool> EmailVerifyUpdate(Guid guid);
}