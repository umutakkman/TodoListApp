namespace TodoListApp.WebApp.Email;

public interface IEmailSender
{
    Task SendEmailAsync(string toEmail, string subject, string message);
}
