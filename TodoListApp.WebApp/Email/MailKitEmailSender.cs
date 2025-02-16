using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace TodoListApp.WebApp.Email;

public class MailSettings
{
    public string Mail { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }
}

public class MailKitEmailSender : IEmailSender
{
    private readonly MailSettings mailSettings;

    public MailKitEmailSender(IOptions<MailSettings> mailSettings)
    {
        this.mailSettings = mailSettings.Value;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(this.mailSettings.Mail);
        email.From.Add(new MailboxAddress(this.mailSettings.DisplayName, this.mailSettings.Mail));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(this.mailSettings.Host, this.mailSettings.Port, MailKit.Security.SecureSocketOptions.Auto);
        await smtp.AuthenticateAsync(this.mailSettings.Mail, this.mailSettings.Password);
        _ = await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
