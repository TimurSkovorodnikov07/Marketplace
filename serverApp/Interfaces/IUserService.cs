public interface IUserService<UserT, UpdatedEntityT> : IEntityService<UserT, UpdatedEntityT>
    where UserT : UserEntity
    where UpdatedEntityT : IUserUpdate
{
    Task<UserT?> Get(Guid guid);
    Task<UserT?> GetConfirmedUser(string email);
    Task<UserT?> GetExistingUser(string email, string passwordHash);
    
    Task Add(UserT newUser);
    Task<bool> Update(UpdatedEntityT updatedUser);
    Task<bool> Remove(Guid guid);
    Task<bool> EmailVerUpdate(Guid guid);
}