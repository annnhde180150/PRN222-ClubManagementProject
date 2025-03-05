using System.Data;
using System.Security.Claims;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ClubManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration, AccountService accountService)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckLogin(string userName, string password)
        {
            var user = await _accountService.CheckLogin(userName, password);
            var adminUsername = _configuration["AdminAccount:Username"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (userName == adminUsername && password == adminPassword)
            {
                string role = "SystemAdmin";
                var accountAdminId = 0;
                HttpContext.Session.SetString("userId", accountAdminId.ToString());
                HttpContext.Session.SetString("username", adminUsername);
                HttpContext.Session.SetString("Role", role);
                return RedirectToAction("Index", "Home");
            }
            if (user != null)
            {
                var clubmember = await _accountService.CheckRole(user.UserId);
                if (clubmember == null)
                {
                    string role = "User";
                    HttpContext.Session.SetString("userId", user.UserId.ToString());
                    HttpContext.Session.SetString("Role", role);
                    return RedirectToAction("Index", "Home");
                }
                if (clubmember.Role.RoleName == "ClubAdmin")
                {
                    string role = "ClubAdmin";
                    HttpContext.Session.SetString("userId", user.UserId.ToString());
                    HttpContext.Session.SetString("Role", role);
                    return RedirectToAction("Index", "Home");
                }
                if (clubmember.Role.RoleName == "ClubMember")
                {
                    string role = "ClubMember";
                    HttpContext.Session.SetString("userId", user.UserId.ToString());
                    HttpContext.Session.SetString("Role", role);
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["ErrorMessage"] = "Invalid email or password. Please try again!";
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public async System.Threading.Tasks.Task GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });            
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var avatar = claims.FirstOrDefault(c => c.Type == "urn:google:picture")?.Value;
            var user = await _accountService.CheckEmailExist(email);
            if (user != null)
            {
                var clubmember = await _accountService.CheckRole(user.UserId);
                if (clubmember == null)
                {
                    string role = "User";
                    HttpContext.Session.SetString("userId", user.UserId.ToString());
                    HttpContext.Session.SetString("Role", role);
                    return RedirectToAction("Index", "Home");
                }
                if (clubmember.Role.RoleName == "ClubAdmin")
                {
                    string role = "ClubAdmin";
                    HttpContext.Session.SetString("userId", user.UserId.ToString());
                    HttpContext.Session.SetString("Role", role);
                    return RedirectToAction("Index", "Home");
                }
                if (clubmember.Role.RoleName == "ClubMember")
                {
                    string role = "ClubMember";
                    HttpContext.Session.SetString("userId", user.UserId.ToString());
                    HttpContext.Session.SetString("Role", role);
                    return RedirectToAction("Index", "Home");
                }
            }
            
            User newUser = new User
            {
                 Username = name,
                 Email = email,
                 PasswordHash ="@123@",
                 ProfilePicture = avatar,
            };
            string roleCheck = "User";
            await _accountService.AddGmailUser(newUser);
            HttpContext.Session.SetString("userId", newUser.UserId.ToString());
            HttpContext.Session.SetString("Role", roleCheck);
            return RedirectToAction("Index", "Home");
        }
    }
}
