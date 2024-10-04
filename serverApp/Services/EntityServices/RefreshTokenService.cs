using Microsoft.EntityFrameworkCore;

public class RefreshTokenService(
    MainDbContext dbContext,
    ILogger<RefreshTokenService> logger)
{
    private readonly ILogger<RefreshTokenService> _logger = logger;
    

    public async Task AddOrUpdate(RefreshTokenEntity refreshTokenEntity)
    {
        var refreshToken = await GetByUserId(refreshTokenEntity.UserId);

        if (refreshToken == null)
        {
            await dbContext.RefreshTokens.AddAsync(refreshTokenEntity);
            await dbContext.SaveChangesAsync();
            return;
        }
        dbContext.RefreshTokens.Update(refreshToken);
        
        //{ Customer
        //   "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIzOGVjMGE0Yi02NDNhLTQyOWItOWI3ZC1jNmZhNDU4ODUyY2UiLCJ1c2VyTmFtZSI6IlRpbXVyIiwidXNlckVtYWlsIjoic2tvdm9yb2RuaWtvdnRpbXVyNzAwMUBnbWFpbC5jb20iLCJ1c2VyVHlwZSI6ImN1c3RvbWVyIiwibmJmIjoxNzI3Nzg3MjQ3LCJleHAiOjE3Mjc3ODgxNDcsImlzcyI6ImxvY2FsaG9zdCJ9.kxbr9WMGaUCMMc4SkFvkbrgtHRGgO9Xnjja3Em7COII",
        //   "refreshToken": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIzOGVjMGE0Yi02NDNhLTQyOWItOWI3ZC1jNmZhNDU4ODUyY2UiLCJ1c2VyTmFtZSI6IlRpbXVyIiwidXNlckVtYWlsIjoic2tvdm9yb2RuaWtvdnRpbXVyNzAwMUBnbWFpbC5jb20iLCJ1c2VyVHlwZSI6ImN1c3RvbWVyIiwibmJmIjoxNzI3Nzg3MjQ3LCJleHAiOjE3Mjg2NTEyNDcsImlzcyI6ImxvY2FsaG9zdCJ9.XFEo4EG0yBAstMH6eTwRYpFl9ZnVpAoZkA1bc4nPFr_xe5ljjouoxhCo9zxnNHkf8itbFagAi74oKxA3mb3nng"
        // }
        
        //{ Seller
        //   "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiJhNjBmN2Y3MS02NzYyLTQ2MGItYmJlMi1mMjNjM2E1YTRiOTkiLCJ1c2VyTmFtZSI6Ik55YXNoa2EiLCJ1c2VyRW1haWwiOiJhbWlybGVnZW5kYTY5QGdtYWlsLmNvbSIsInVzZXJUeXBlIjoic2VsbGVyIiwibmJmIjoxNzI3Nzg3MjAyLCJleHAiOjE3Mjc3ODgxMDIsImlzcyI6ImxvY2FsaG9zdCJ9.SgZ2CZf0db_zlD2g7kT5Thk5BbW7hyw2UkQ4MeUBy-4",
        //   "refreshToken": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiJhNjBmN2Y3MS02NzYyLTQ2MGItYmJlMi1mMjNjM2E1YTRiOTkiLCJ1c2VyTmFtZSI6Ik55YXNoa2EiLCJ1c2VyRW1haWwiOiJhbWlybGVnZW5kYTY5QGdtYWlsLmNvbSIsInVzZXJUeXBlIjoic2VsbGVyIiwibmJmIjoxNzI3Nzg3MjAyLCJleHAiOjE3Mjg2NTEyMDIsImlzcyI6ImxvY2FsaG9zdCJ9.MJst289otzrqco2Kh30yo_nnnibyQQApY5MSGv_VbGKVfidQj3BqfJLi-zO8Y2TL4noU-dG1O6Px2AwUGMoXNQ"
        // }
    }
    public async Task Update(RefreshTokenEntity refreshTokenEntity)
    {
        dbContext.RefreshTokens.Update(refreshTokenEntity);
    }

    public async Task<RefreshTokenEntity?> GetByUserId(Guid userId)
    {
        return await dbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
}