using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using server_app.Application.Abstractions.EmailSend;
using server_app.Domain.Model.Options;

namespace server_app.Application.Services.MailServices;

public class EmailSenderByYandexService(
    IOptions<EmailOptions> options,
    ILogger<EmailSenderByYandexService> logger,
    BaseEmailSenderService baseSender)
    : IEmailSender
{
    private readonly EmailOptions _emailOptions = options.Value;
    private readonly ILogger<EmailSenderByYandexService> _logger = logger;

    public async Task SendAsync(string toAddress, string title, string htmlBody)
    {
        await baseSender.Send(_emailOptions.Address, _emailOptions.Password, toAddress,
            "smtp.yandex.ru", 587, title, htmlBody);
    }
}