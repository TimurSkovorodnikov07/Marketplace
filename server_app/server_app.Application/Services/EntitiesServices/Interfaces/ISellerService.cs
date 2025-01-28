using server_app.Domain.Entities.Users.Seller;
using server_app.Domain.Model.Dtos;

namespace server_app.Application.Services.EntitiesServices.Interfaces;

public interface ISellerService : IUserService<SellerEntity, UserUpdateDto>
{
    
}