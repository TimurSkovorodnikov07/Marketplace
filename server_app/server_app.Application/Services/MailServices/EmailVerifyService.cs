using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using server_app.Application.Abstractions.EmailSend;
using server_app.Application.Abstractions.Hashing;
using server_app.Domain.Model.Options;

namespace server_app.Application.Services.MailServices;

public class EmailVerifyService(
    IOptions<VerfiyCodeOptions> options,
    ILogger<EmailVerifyService> logger,
    IDistributedCache distributedCache,
    IEmailSender emailSend,
    ICodeCreator codeCreator,
    IHasher hasher,
    IHashVerify hashVerify)
    : IEmailVerify
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

        return code is not null && codeInCache is not null
                                && hashVerify.Verify(code, codeInCache)
            ? true : false;
    }
}