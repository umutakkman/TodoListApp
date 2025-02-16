using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.ViewModels;

/// <summary>
/// View model for forgot password functionality.
/// </summary>
public class ForgotPasswordViewModel
{
    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
