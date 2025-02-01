using Microsoft.EntityFrameworkCore;
using server_app.Application.Repositories;
using server_app.Domain.Users.Tokens;

namespace server_app.Infrastructure.Repositories.Users;

public class RefreshTokenRepository(
    MainDbContext dbContext,
    IUserRepository userRepository) : IRefreshTokenRepository
{
    public async Task AddOrUpdate(RefreshTokenEntity refreshTokenEntity)
    {
        var refreshToken = await GetByUserId(refreshTokenEntity.UserId);

        if (refreshToken == null)
        {
            var userExist = await userRepository.EmailVerifyUpdate(refreshTokenEntity.UserId);

            if (userExist)
            {
                await dbContext.RefreshTokens.AddAsync(refreshTokenEntity);
                await dbContext.SaveChangesAsync();
                return;
            }
        }

        await Update(refreshTokenEntity);
    }

    public async Task Update(RefreshTokenEntity updatedToken)
    {
        dbContext.RefreshTokens.Update(updatedToken);
    }

    public async Task<RefreshTokenEntity?> GetByUserId(Guid userId)
    {
        return await dbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
}