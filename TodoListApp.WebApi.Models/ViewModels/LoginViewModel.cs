using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.ViewModels;

/// <summary>
/// View model for login functionality.
/// </summary>
public class LoginViewModel
{
    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether to remember the user.
    /// </summary>
    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }

    /// <summary>
    /// Gets or sets the return URL.
    /// </summary>
    public string? ReturnUrl { get; set; }
}
