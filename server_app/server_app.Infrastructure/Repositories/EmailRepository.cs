using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using server_app.Application.Abstractions.EmailSend;
using server_app.Application.Abstractions.Hashing;
using server_app.Application.Options;

namespace server_app.Infrastructure.Repositories;

public class EmailRepository(
    IOptions<VerfiyCodeOptions> options,
    ILogger<EmailRepository> logger,
    IDistributedCache distributedCache,
    IEmailSender emailSend,
    ICodeCreator codeCreator,
    IHasher hasher,
    IHashVerify hashVerify)
    : IEmailRepository
{
    private readonly VerfiyCodeOptions _options = options.Value;

    public async Task CodeSend(Guid userId, string email)
    {
        var code = codeCreator.Create(_options.Length);
        var hashedCode = hasher.Hashing(code);

        await distributedCache.SetStringAsync(userId.ToString(), hashedCode,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_options.DiedAfterSeconds)
            });

        string title = _options.EmailMessageTitle;
        string htmlBody = _options.EmailMessageHtmlBody.Replace("{CODE}", code.ToString());

        logger.LogDebug("The message was sent to email: " + email);
        await emailSend.SendAsync(email, title, htmlBody);
    }

    public async Task Resend(Guid userId, string email)
    {
        await distributedCache.RemoveAsync(userId.ToString());
        await CodeSend(userId, email);
    }

    public async Task<bool> CodeVerify(Guid userId, string code)
    {
        var codeInCache = await distributedCache.GetStringAsync(userId.ToString());

        return string.IsNullOrEmpty(code) == false
               && string.IsNullOrEmpty(codeInCache) == false
               && hashVerify.Verify(code, codeInCache);
    }
}