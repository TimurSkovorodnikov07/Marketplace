using server_app.Domain.Users.Tokens;

namespace server_app.Application.Repositories;

public interface IRefreshTokenRepository
{
    Task AddOrUpdate(RefreshTokenEntity refreshTokenEntity);
    Task Update(RefreshTokenEntity updatedToken);
    Task<RefreshTokenEntity?> GetByUserId(Guid userId);
}