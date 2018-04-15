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
using System.Text;

namespace BryceFamily.Web.MVC.Controllers
{
    public class AccountController : BaseController
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
            ILogger<AccountController> logger):base("Accounts", "accounts")
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
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public async Task<IActionResult> RoleManager()
        {
            var cancellationToken = CancellationToken.None;
            return View((await GetUserRolesList(cancellationToken)).OrderBy(t => t.Email));
        }


        [HttpPost]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
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


        [HttpGet]
        public IActionResult RegistrationRequest()
        {
            ViewBag.Success = false;
            return View(new RegistrationRequest());
        }


        [HttpPost]
        public async Task<IActionResult> RegistrationRequest(RegistrationRequest request)
        {
            try
            {
                var cancellationToken = GetCancellationToken();

                var userExists = await _userManager.FindByEmailAsync(request.Email);

                if (userExists != null)
                {
                    var urlLink = Url.Action("ForgotPassword", "Account");
                    ViewData["message"] = $"A user has already been registered with this email address. If you are this person, head <a href=\"{urlLink}\"> here </a> to reset your password.";
                    ViewBag.Success = false;
                }
                else
                {
                    var adminUsers = (await _roleManager.GetUserIdsInRoleAsync(RoleNameConstants.AdminRole, cancellationToken)).ToList();
                    var superAdmminUsers = (await _roleManager.GetUserIdsInRoleAsync(RoleNameConstants.SuperAdminRole, cancellationToken)).ToList();

                    var emails = new List<string>();
                    foreach (var userId in (adminUsers.Union(superAdmminUsers)))
                    {
                        var user = await _userManager.FindByIdAsync(userId);
                        emails.Add(user.Email.Value);
                    }

                    await _sesService.SendBulkEmail(emails.AsEnumerable(), GetRequestEmail(request), "New User Request", cancellationToken);
                    ViewData["message"] = "Thanks, your request has been sent to the web site admins one of whom will respond as soon as they're able to.";
                    ViewBag.Success = true;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                _logger.LogError(ex, $"There was an error handling the registration request for {request.Email}");
                ViewData["message"] = "Unfortunately there was an error handling your request. The web admins have been notified";

            }

            return View(request);
        }

        private string GetRequestEmail(RegistrationRequest request)
        {
            var sb = new StringBuilder();
            var url = Url.Action("AutoApprove", "Account", new { emailToApprove = request.Email }, protocol: HttpContext.Request.Scheme);

            sb.Append("<table border=\"0\" valign=\"top\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"><thead><tr><th>Bryce Family Web Site has received a new request for access</th></tr></thead>");
            sb.Append($"<tbody><tr><td>First Name:</td><td>{request.FirstName}</td></tr>");
            sb.Append($"<tr><td>Last Name:</td><td>{request.LastName}</td></tr>");
            sb.Append($"<tr><td>Email:</td><td>{request.Email}</td></tr>");
            sb.Append($"<tr><td>Click to approve:</td><td><a href=\"{url}\" target=\"_blank\">Approve</a></td></tr>");
            sb.Append($"</table>");
            return sb.ToString();

            
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
            var users = new List<RoleManagerViewModel>();
            var readonlyUsers = (await _roleManager.GetUserIdsInRoleAsync(RoleNameConstants.UserRole, cancellationToken)).ToList();

            var adminUsers = (await _roleManager.GetUserIdsInRoleAsync(RoleNameConstants.AdminRole, cancellationToken)).ToList();
            readonlyUsers.AddRange(adminUsers);


            if (_contextService.IsSuperUser())
            {
                var superAdminUsers = (await _roleManager.GetUserIdsInRoleAsync(RoleNameConstants.SuperAdminRole, cancellationToken)).ToList();
                readonlyUsers.AddRange(superAdminUsers);
            }

           
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

            return users.Where(t => t.Email != User.Identity.Name); // filter out the current logged in user from the list;
        }

        [HttpGet("Account/AutoApprove/{emailToApprove}"), Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public async Task<IActionResult> AutoApprove(string emailToApprove)
        {
            var cancellationToken = GetCancellationToken();
            var existingUser = await _userManager.FindByEmailAsync(emailToApprove);

            if (existingUser != null)
            {
                ViewData["message"] = $"A user has already been registered with this email - perhaps another admin has already handled this request [{emailToApprove}]";
                return View();
            }


            var user = new DynamoIdentityUser(emailToApprove, emailToApprove);
            var result = await _userManager.CreateAsync(user, Guid.NewGuid().ToString().ToUpper() + Guid.NewGuid().ToString().ToLower());
            if (result.Succeeded)
            {
                await _roleManager.AddToRoleAsync(user, "user", CancellationToken.None);
                _logger.LogInformation(3, "User created a new account with password.");
                ViewData["message"] = $"User [{emailToApprove}] created and login email has been sent";
            }
            else
            {
                AddErrors(result);
                ViewData["message"] = string.Join("<br />", result.Errors.Select(e => e.Description));
            }

            //set up the reset password link
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
            await _sesService.SendEmail(emailToApprove, GetRegistrationEmail(emailToApprove, callbackUrl), "Bryce Family Website Registration", cancellationToken);
            return View();
        }


        private static string GetRegistrationEmail(string emailToApprove, string callbackUrl)
        {

            var sb = new StringBuilder();

            sb.Append("<table border=\"0\" valign=\"top\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"><thead><tr><th>You have been successfully registered on the Bryce Family Website</th></tr></thead>");
            sb.Append($"<tbody><tr><td><p>Welcome! You have been successfully registered for access to the Bryce Family Website and you now have access to the contact lists etc contained within the site.</p>");
            sb.Append($"<tbody><tr><td><p>Your username is the email address you entered in the request which is {emailToApprove}</p>");
            sb.Append("<p>In order to complete your access, you will need to set up a password. In order to do this, click on the link below.</p>");
            sb.Append($"<p><a href=\"{callbackUrl}\">Click Me</a></p>");
            sb.Append($"<p>This link is valid for 24 hours. If the link has expired, simply head <a href=\"https://www.brycefamily.net/Account/ForgotPassword\" >here</a> and request a new password.</p>");
            sb.Append("</td></tr></table>");

            return sb.ToString();
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