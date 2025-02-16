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

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign-in manager.</param>
        /// <param name="emailSender">The email sender.</param>
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
        }

        /// <summary>
        /// Displays the registration page.
        /// </summary>
        /// <returns>The registration view.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        /// <summary>
        /// Handles the registration form submission.
        /// </summary>
        /// <param name="model">The registration view model.</param>
        /// <returns>The result of the registration process.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var validationResult = this.ValidateRegisterModel(model);
            if (validationResult != null)
            {
                return validationResult;
            }

            ArgumentNullException.ThrowIfNull(model);

            return await this.RegisterUserAsync(model);
        }

        /// <summary>
        /// Displays the login page.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>The login view.</returns>
        [HttpGet]
        public IActionResult Login(Uri? returnUrl = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return this.View(model);
        }

        /// <summary>
        /// Handles the login form submission.
        /// </summary>
        /// <param name="model">The login view model.</param>
        /// <returns>The result of the login process.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var validationResult = this.ValidateLoginModel(model);
            if (validationResult != null)
            {
                return validationResult;
            }

            ArgumentNullException.ThrowIfNull(model);

            return await this.HandleLoginAsync(model);
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>The login view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return this.RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Displays the forgot password page.
        /// </summary>
        /// <returns>The forgot password view.</returns>
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return this.View();
        }

        /// <summary>
        /// Handles the forgot password form submission.
        /// </summary>
        /// <param name="model">The forgot password view model.</param>
        /// <returns>The result of the forgot password process.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var validationResult = this.ValidateForgotPasswordModel(model);
            if (validationResult != null)
            {
                return validationResult;
            }

            ArgumentNullException.ThrowIfNull(model);

            return await this.HandleForgotPasswordAsync(model);
        }

        /// <summary>
        /// Displays the forgot password confirmation page.
        /// </summary>
        /// <returns>The forgot password confirmation view.</returns>
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return this.View();
        }

        /// <summary>
        /// Displays the reset password page.
        /// </summary>
        /// <param name="code">The reset code.</param>
        /// <param name="email">The email address.</param>
        /// <returns>The reset password view.</returns>
        [HttpGet]
        public IActionResult ResetPassword(string code = null!, string email = null!)
        {
            if (code == null || email == null)
            {
                return this.BadRequest("A code and email must be supplied for password reset.");
            }

            var model = new ResetPasswordViewModel { Code = code, Email = email };
            return this.View(model);
        }

        /// <summary>
        /// Handles the reset password form submission.
        /// </summary>
        /// <param name="model">The reset password view model.</param>
        /// <returns>The result of the reset password process.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var validationResult = this.ValidateResetPasswordModel(model);
            if (validationResult != null)
            {
                return validationResult;
            }

            ArgumentNullException.ThrowIfNull(model);

            return await this.ResetPasswordAsync(model);
        }

        /// <summary>
        /// Displays the reset password confirmation page.
        /// </summary>
        /// <returns>The reset password confirmation view.</returns>
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return this.View();
        }

        /// <summary>
        /// Registers a new user asynchronously.
        /// </summary>
        /// <param name="model">The registration view model.</param>
        /// <returns>The result of the registration process.</returns>
        private async Task<IActionResult> RegisterUserAsync(RegisterViewModel model)
        {
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

            return this.View("Register", model);
        }

        /// <summary>
        /// Handles the login process asynchronously.
        /// </summary>
        /// <param name="model">The login view model.</param>
        /// <returns>The result of the login process.</returns>
        private async Task<IActionResult> HandleLoginAsync(LoginViewModel model)
        {
            var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (model.ReturnUrl != null && this.Url.IsLocalUrl(model.ReturnUrl.ToString()))
                {
                    return this.Redirect(model.ReturnUrl.ToString());
                }

                return this.RedirectToAction("Index", "TodoList");
            }

            this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return this.View("Login", model);
        }

        /// <summary>
        /// Handles the forgot password process asynchronously.
        /// </summary>
        /// <param name="model">The forgot password view model.</param>
        /// <returns>The result of the forgot password process.</returns>
        private async Task<IActionResult> HandleForgotPasswordAsync(ForgotPasswordViewModel model)
        {
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

        /// <summary>
        /// Handles the reset password process asynchronously.
        /// </summary>
        /// <param name="model">The reset password view model.</param>
        /// <returns>The result of the reset password process.</returns>
        private async Task<IActionResult> ResetPasswordAsync(ResetPasswordViewModel model)
        {
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

            return this.View("ResetPassword", model);
        }

        /// <summary>
        /// Validates the registration view model.
        /// </summary>
        /// <param name="model">The registration view model.</param>
        /// <returns>The validation result.</returns>
        private ViewResult? ValidateRegisterModel(RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            ArgumentNullException.ThrowIfNull(model);
            return null;
        }

        /// <summary>
        /// Validates the login view model.
        /// </summary>
        /// <param name="model">The login view model.</param>
        /// <returns>The validation result.</returns>
        private ViewResult? ValidateLoginModel(LoginViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            ArgumentNullException.ThrowIfNull(model);
            return null;
        }

        /// <summary>
        /// Validates the forgot password view model.
        /// </summary>
        /// <param name="model">The forgot password view model.</param>
        /// <returns>The validation result.</returns>
        private ViewResult? ValidateForgotPasswordModel(ForgotPasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            ArgumentNullException.ThrowIfNull(model);
            return null;
        }

        /// <summary>
        /// Validates the reset password view model.
        /// </summary>
        /// <param name="model">The reset password view model.</param>
        /// <returns>The validation result.</returns>
        private ViewResult? ValidateResetPasswordModel(ResetPasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            ArgumentNullException.ThrowIfNull(model);
            return null;
        }
    }
}
