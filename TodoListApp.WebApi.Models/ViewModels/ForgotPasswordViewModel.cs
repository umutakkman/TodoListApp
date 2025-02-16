using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
