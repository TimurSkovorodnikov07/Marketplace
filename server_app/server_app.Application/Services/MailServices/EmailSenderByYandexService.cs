using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using server_app.Application.Abstractions.EmailSend;
using server_app.Application.Options;

namespace server_app.Application.Services.MailServices;

public class EmailSenderByYandexService(
    IOptions<EmailOptions> options,
    ILogger<EmailSenderByYandexService> logger,
    BaseEmailSenderService baseSender)
    : IEmailSender
{
    private readonly EmailOptions _emailOptions = options.Value;
    private readonly ILogger<EmailSenderByYandexService> _logger = logger;
    
    private const string Host = "smtp.yandex.ru";
    
    public async Task SendAsync(string toAddress, string title, string htmlBody)
    {
        await baseSender.Send(_emailOptions.Address, _emailOptions.Password, toAddress,
            Host, 587, title, htmlBody);
    }
}