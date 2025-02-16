using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.ViewModels;

/// <summary>
/// View model for resetting the password.
/// </summary>
public class ResetPasswordViewModel
{
    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new password.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the confirmation password.
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the reset code.
    /// </summary>
    public string Code { get; set; } = string.Empty;
}
