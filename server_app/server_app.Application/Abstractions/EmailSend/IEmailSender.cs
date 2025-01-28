namespace server_app.Application.Abstractions.EmailSend;

public interface IEmailSender
{
    public Task SendAsync(string toAddress, string title, string htmlBody);
}