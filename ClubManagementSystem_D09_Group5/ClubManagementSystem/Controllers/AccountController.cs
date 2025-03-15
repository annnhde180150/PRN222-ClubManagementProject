using System.Data;
using System.Security.Claims;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace ClubManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration, IAccountService accountService)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _accountService.FindUserAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
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
                Password = "@123@",
                //ProfilePicture = avatar,
            };
            string roleCheck = "User";
            await _accountService.AddGmailUser(newUser);
            HttpContext.Session.SetString("userId", newUser.UserId.ToString());
            HttpContext.Session.SetString("Role", roleCheck);
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user); // Return the view to show validation messages
            }

            var emailValid = await _accountService.CheckEmailExist(user.Email);
            var usernameValid = await _accountService.CheckUsernameExist(user.Username);

            if (emailValid != null)
            {
                TempData["ErrorMessage"] = "Email already exists!";
                return RedirectToAction("SignUp", "Account");
            } else if (usernameValid != null)
            {
                TempData["ErrorMessage"] = "Username already exists!";
                return RedirectToAction("SignUp", "Account");
            }
                user.CreatedAt = DateTime.Now;

            await _accountService.AddUser(user);

            //TempData["SuccessMessage"] = "Sign up successfully!";
            return RedirectToAction("Login", "Account");
        }


        // POST: Save changes to profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountService.FindUserAsync(model.UserId);

                if (user == null)
                {
                    return NotFound();
                }

                // Update the user's details
                user.Username = model.Username;
                user.Email = model.Email;

                // If a new password is provided, check if it matches the confirm password and update
                if (!string.IsNullOrEmpty(model.Password) && model.Password == model.ConfirmPassword)
                {
                    user.Password = model.Password; // Save the password as plain text
                }
                else if (!string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("Password", "Passwords do not match.");
                    return View(model); // Return the view if passwords do not match
                }

                await _accountService.UpdateUserAsync(user); // Assuming this method updates the user in the database

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return View(model); // Or redirect to the profile page
            }

            return View(model); // Return the view with validation errors
        }
    }
}


