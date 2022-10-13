// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ProjectWorkDemo.Areas.Identity.Data;
using ProjectWorkDemo.Models;

namespace ProjectWorkDemo.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ProjectWorkDatabaseContext _context;
        private readonly SignInManager<ProjectWorkDemoUser> _signInManager;
        private readonly UserManager<ProjectWorkDemoUser> _userManager;
        private readonly IUserStore<ProjectWorkDemoUser> _userStore;
        private readonly IUserEmailStore<ProjectWorkDemoUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(ProjectWorkDatabaseContext context,
            UserManager<ProjectWorkDemoUser> userManager,
            IUserStore<ProjectWorkDemoUser> userStore,
            SignInManager<ProjectWorkDemoUser> signInManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Key]
            public int IdUtente { get; set; }
            [Required(ErrorMessage = "Please enter Nome")]
            [Display(Name = "Nome")]
            public string Nome { get; set; }
            [Required(ErrorMessage = "Please enter Cognome")]
            [Display(Name = "Cognome")]
            public string Cognome { get; set; }
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Telefono")]
            [Required(ErrorMessage = "Telefono Required!")]
            [StringLength(10)]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered telefono format is not valid: 10 numbers required.")]
            public string Telefono { get; set; }
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "min length 6 letters, at least 1 upper letter, a number and a non-alphanumeric symbol", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfermaPw { get; set; }
            public string RoleId { get; set; } = null!;
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Name");
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(AspNetUserRole userRole, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Nome = Input.Nome;
                user.Cognome = Input.Cognome;
                user.Telefono = Input.Telefono;
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);
                userRole = new AspNetUserRole()
                {
                    UserId = user.Id,
                    RoleId = Input.RoleId
                };
                _context.Add(userRole);
                await _context.SaveChangesAsync();
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    var defaultrole = _roleManager.FindByNameAsync("Default").Result;
                    if (defaultrole != null)
                    {
                        IdentityResult roleresult = await _userManager.AddToRoleAsync(user, defaultrole.Name);
                    }

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If we got this far, something failed, redisplay form
            ViewData["RoleId"] = new SelectList(_context.AspNetUsers, "Id", "Nome");
            return Page();
        }

        private ProjectWorkDemoUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ProjectWorkDemoUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ProjectWorkDemoUser)}'. " +
                    $"Ensure that '{nameof(ProjectWorkDemoUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ProjectWorkDemoUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ProjectWorkDemoUser>)_userStore;
        }
    }
}
