using Microsoft.EntityFrameworkCore;

public class RefreshTokenService(
    MainDbContext dbContext,
    UserEntityService userEntityService)
{
    public async Task AddOrUpdate(RefreshTokenEntity refreshTokenEntity)
    {
        var refreshToken = await GetByUserId(refreshTokenEntity.UserId);

        if (refreshToken == null)
        {
            var userExist = await userEntityService.EmailVerifyUpdate(refreshTokenEntity.UserId);
            
            if (userExist)
            {
                await dbContext.RefreshTokens.AddAsync(refreshTokenEntity);
                await dbContext.SaveChangesAsync();
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