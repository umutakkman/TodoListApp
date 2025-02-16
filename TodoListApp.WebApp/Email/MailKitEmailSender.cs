using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace TodoListApp.WebApp.Email;

/// <summary>
/// Settings for configuring the email sender.
/// </summary>
public class MailSettings
{
    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Mail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the SMTP host.
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the SMTP port.
    /// </summary>
    public int Port { get; set; }
}

/// <summary>
/// Implementation of <see cref="IEmailSender"/> using MailKit.
/// </summary>
public class MailKitEmailSender : IEmailSender
{
    private readonly MailSettings mailSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="MailKitEmailSender"/> class.
    /// </summary>
    /// <param name="mailSettings">The mail settings.</param>
    public MailKitEmailSender(IOptions<MailSettings> mailSettings)
    {
        ArgumentNullException.ThrowIfNull(mailSettings);
        this.mailSettings = mailSettings.Value;
    }

    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="toEmail">The recipient email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="message">The message body of the email.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        using var email = new MimeMessage
        {
            Sender = MailboxAddress.Parse(this.mailSettings.Mail),
        };
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
