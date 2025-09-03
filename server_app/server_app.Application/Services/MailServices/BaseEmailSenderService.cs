using Microsoft.Extensions.Logging;
using MimeKit;

namespace server_app.Application.Services.MailServices;

public class BaseEmailSenderService(ILogger<BaseEmailSenderService> logger)
{
    private readonly ILogger<BaseEmailSenderService> _logger = logger;

    public async Task Send(
        string fromAddress,
        string emailPas,
        string toAddress,
        string host,
        int port,
        string title,
        string htmlBody
    )
    {
        using var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Admin", fromAddress));
        message.To.Add(new MailboxAddress("", toAddress));
        message.Subject = title;
        message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlBody };

        using var client = new MailKit.Net.Smtp.SmtpClient();
        await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(fromAddress, emailPas);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
