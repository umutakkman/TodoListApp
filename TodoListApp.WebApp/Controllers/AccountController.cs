using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models.ViewModels;
using TodoListApp.WebApp.Email;

namespace TodoListApp.WebApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IEmailSender emailSender;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            ArgumentNullException.ThrowIfNull(model);

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await this.userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await this.signInManager.SignInAsync(user, isPersistent: false);
                return this.RedirectToAction("Index", "TodoList");
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }

            return this.View(model);
        }

        public IActionResult Login(string? returnUrl = null)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            ArgumentNullException.ThrowIfNull(model);

            var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && this.Url.IsLocalUrl(model.ReturnUrl))
                {
                    return this.Redirect(model.ReturnUrl);
                }

                return this.RedirectToAction("Index", "TodoList");
            }

            this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return this.RedirectToAction("Login", "Account");
        }

        public IActionResult ForgotPassword()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            ArgumentNullException.ThrowIfNull(model);

            var user = await this.userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return this.RedirectToAction(nameof(this.ForgotPasswordConfirmation));
            }

            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = this.Url.Action(nameof(this.ResetPassword), "Account", new { code = token, email = model.Email }, protocol: this.Request.Scheme);

            var emailSubject = "Reset Password";
            var emailMessage = $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>";
            await this.emailSender.SendEmailAsync(model.Email, emailSubject, emailMessage);

            return this.RedirectToAction(nameof(this.ForgotPasswordConfirmation));
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return this.View();
        }

        public IActionResult ResetPassword(string code = null!, string email = null!)
        {
            if (code == null || email == null)
            {
                return this.BadRequest("A code and email must be supplied for password reset.");
            }

            var model = new ResetPasswordViewModel { Code = code, Email = email };
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            ArgumentNullException.ThrowIfNull(model);

            var user = await this.userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return this.RedirectToAction(nameof(this.ResetPasswordConfirmation));
            }

            var result = await this.userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return this.RedirectToAction(nameof(this.ResetPasswordConfirmation));
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }

            return this.View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return this.View();
        }
    }
}
