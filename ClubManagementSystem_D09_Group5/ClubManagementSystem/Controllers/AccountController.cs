using Microsoft.AspNetCore.Authorization;
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
                if(clubmember == null)
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
        public IActionResult GoogleLogin()
        {
            return RedirectToAction("Account" ,"Login");
        }
    }
}
