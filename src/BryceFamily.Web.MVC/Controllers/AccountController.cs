using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using BryceFamily.Web.MVC.Models.AccountViewModels;
using AspNetCore.Identity.DynamoDB;
using System;
using BryceFamily.Web.MVC.Infrastructure.Authentication;
using BryceFamily.Web.MVC.Infrastructure;
using BryceFamily.Repo.Core.Emails;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace BryceFamily.Web.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<DynamoIdentityUser> _signInManager;
        private readonly ContextService _contextService;
        private readonly ISesService _sesService;
        private readonly ClanAndPeopleService _clanAndPeopleService;
        private readonly DynamoRoleUsersStore<DynamoIdentityRole, DynamoIdentityUser> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<DynamoIdentityUser> _userManager;

        public AccountController(
            UserManager<DynamoIdentityUser> userManager,
            SignInManager<DynamoIdentityUser> signInManager,
            ContextService contextService,
            ISesService sesService,
            ClanAndPeopleService clanAndPeopleService,
            DynamoRoleUsersStore<DynamoIdentityRole, DynamoIdentityUser> roleManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _contextService = contextService;
            _sesService = sesService;
            _clanAndPeopleService = clanAndPeopleService;
            _roleManager = roleManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                try
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation(1, "User logged in.");

                        return RedirectToLocal(returnUrl);
                    }

                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning(2, "User account locked out.");
                        return View("Lockout");
                    }
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Could not verify user login attempt", ex);
                    throw;
                }

             
                return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [Authorize(Roles = RoleNameConstants.SuperAdminRole)]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpPost]
        [Authorize(Roles = RoleNameConstants.SuperAdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new DynamoIdentityUser(model.Email, model.Email);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await _roleManager.AddToRoleAsync(user, "user", CancellationToken.None);
                    await _signInManager.SignInAsync(user, false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LogOff()
        {
            await Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, IdentityConstants.ApplicationScheme);
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                await _sesService.SendEmail(model.Email, 
                                            "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>",
                                            "Email Reset",
                                            CancellationToken.None);
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = RoleNameConstants.SuperAdminRole)]
        public async Task<IActionResult> RoleManager()
        {
            var cancellationToken = CancellationToken.None;
            return View((await GetUserRolesList(cancellationToken)).OrderBy(t => t.Email));
        }


        [HttpPost]
        [Authorize(Roles = RoleNameConstants.SuperAdminRole)]
        public async Task<IActionResult> RoleManager(IEnumerable<RoleManagerViewModel> roles)
        {
            var cancellationToken = CancellationToken.None;

            var currentRoles = await GetUserRolesList(cancellationToken);

            foreach (var role in roles)
            {
                var current = currentRoles.FirstOrDefault(r => r.Id == role.Id);
                if (current.IsInSuperUserRole != role.IsInSuperUserRole)
                {
                    if (role.IsInSuperUserRole)
                    {
                        role.IsInEditorRole = false;
                        role.IsInUserRole = false;
                    }
                    await UpdateRole(role.Id, RoleNameConstants.SuperAdminRole, !role.IsInSuperUserRole, cancellationToken);
                }
                if (current.IsInEditorRole != role.IsInEditorRole)
                {
                    if (role.IsInEditorRole)
                    {
                        role.IsInUserRole = false;
                    }
                    await UpdateRole(role.Id, RoleNameConstants.AdminRole, !role.IsInEditorRole, cancellationToken);
                }
                if (!(role.IsInSuperUserRole || (role.IsInEditorRole)))
                {
                    role.IsInUserRole = true;
                }
                // make sure we are at least set the user to readonly mode 
                if (current.IsInUserRole != role.IsInUserRole)
                {
                    await UpdateRole(role.Id, RoleNameConstants.UserRole, !role.IsInUserRole, cancellationToken);
                }
            }

            return RedirectToAction("RoleManager");
        }


        private async Task UpdateRole(string userId, string roleName, bool remove, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (remove)
                await _roleManager.RemoveFromRoleAsync(user, roleName, cancellationToken);
            else
                await _roleManager.AddToRoleAsync(user, roleName, cancellationToken);
        }

        private async Task<IEnumerable<RoleManagerViewModel>> GetUserRolesList(CancellationToken cancellationToken)
        {
            var readonlyUsers = (await _roleManager.GetUserIdsInRoleAsync(RoleNameConstants.UserRole, cancellationToken)).ToList();
            var adminUsers = (await _roleManager.GetUserIdsInRoleAsync(RoleNameConstants.AdminRole, cancellationToken)).ToList();
            var superAdminUsers = (await _roleManager.GetUserIdsInRoleAsync(RoleNameConstants.SuperAdminRole, cancellationToken)).ToList();

            var users = new List<RoleManagerViewModel>();

            readonlyUsers.AddRange(adminUsers);
            readonlyUsers.AddRange(superAdminUsers);
            foreach (var user in readonlyUsers.Distinct())
            {
                var x = await _userManager.FindByIdAsync(user);
                users.Add(new RoleManagerViewModel()
                {
                    Email = x.UserName,
                    IsInUserRole = await _roleManager.IsInRoleAsync(x, RoleNameConstants.UserRole, cancellationToken),
                    IsInEditorRole = await _roleManager.IsInRoleAsync(x, RoleNameConstants.AdminRole, cancellationToken),
                    IsInSuperUserRole = await _roleManager.IsInRoleAsync(x, RoleNameConstants.SuperAdminRole, cancellationToken),
                    Id = x.Id
                });
            }

            return users;
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }


    }
}