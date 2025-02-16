namespace TodoListApp.WebApp.Email;

/// <summary>
/// Interface for sending emails.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="toEmail">The recipient email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="message">The message body of the email.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendEmailAsync(string toEmail, string subject, string message);
}
