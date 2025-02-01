using server_app.Domain.Entities.Users.Seller;
using server_app.Domain.Model.Dtos;

namespace server_app.Application.Repositories;

public interface ISellerRepository : IUserRepository<SellerEntity, UserUpdateDto>
{
        
}