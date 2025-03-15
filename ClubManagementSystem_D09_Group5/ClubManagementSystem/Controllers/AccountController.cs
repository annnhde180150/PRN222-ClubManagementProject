using System.Data;
using System.Security.Claims;
using System.Text;
using BussinessObjects.Models;
using ClubManagementSystem.Controllers.Filter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Implementation;
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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckLogin(string userName, string password)
        {
            var user = await _accountService.CheckLogin(userName, password);
            var adminGmail = _configuration["AdminAccount:Gmail"];
            var adminPassword = _configuration["AdminAccount:Password"];
            string? profilePicture = "";
            if (userName == adminGmail && password == adminPassword)
            {
                string role = "SystemAdmin";
                var accountAdminId = 0;
                var adminName = "Admin";
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, accountAdminId.ToString()),
                    new Claim(ClaimTypes.Name, adminName),
                    new Claim(ClaimTypes.Email, adminGmail),
                    new Claim(ClaimTypes.Role, role),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));               
                HttpContext.Session.SetString("userPicture", profilePicture);
                return RedirectToAction("Index", "Home");
            }
            if (user != null)
            {
                var clubmember = await _accountService.CheckRole(user.UserId);
                
                if (user.ProfilePicture != null)
                {
                    profilePicture = Encoding.UTF8.GetString(user.ProfilePicture);
                }              
                if (clubmember == null)
                {
                    string role = "User";
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, role),
                };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    HttpContext.Session.SetString("userPicture", profilePicture);
                    return RedirectToAction("Index", "Home");
                }
                if (clubmember.Role.RoleName == "ClubAdmin")
                {
                    string role = "ClubAdmin";
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, role),
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    HttpContext.Session.SetString("userPicture", profilePicture);
                    return RedirectToAction("Index", "Home");
                }
                if (clubmember.Role.RoleName == "ClubMember")
                {
                    string role = "ClubMember";
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, role),
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    HttpContext.Session.SetString("userPicture", profilePicture);
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
            var GoogleClaims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value,               
            });
            var email = GoogleClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = GoogleClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            string avatar = result.Principal.FindFirst("urn:google:picture")?.Value;
            var user = await _accountService.CheckEmailExist(email);
            byte[] avatarByte = Encoding.UTF8.GetBytes(avatar);
            var claims = new List<Claim>();
            string profilePicture = "";
            ClaimsIdentity claimsIdentity;
            if (user != null)
            {
                var clubmember = await _accountService.CheckRole(user.UserId);
                profilePicture = Encoding.UTF8.GetString(user.ProfilePicture);
                if (clubmember == null)
                {
                    string role = "User";
                    claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, role),
                    };
                    claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    HttpContext.Session.SetString("userPicture", profilePicture);
                    return RedirectToAction("Index", "Home");
                }
                if (clubmember.Role.RoleName == "ClubAdmin")
                {
                    string role = "ClubAdmin";
                    claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, role),
                    };
                    claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Index", "Home");
                }
                if (clubmember.Role.RoleName == "ClubMember")
                {
                    string role = "ClubMember";
                    claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, role),
                    };
                    claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    HttpContext.Session.SetString("userPicture", profilePicture);
                    return RedirectToAction("Index", "Home");
                }
            }

            User newUser = new User
            {
                Username = name,
                Email = email,
                ProfilePicture = avatarByte,
                Password = "@123@"
            };
            string roleCheck = "User";
            var newGmailUser = await _accountService.AddGmailUser(newUser);
            profilePicture = Encoding.UTF8.GetString(newGmailUser.ProfilePicture);
            claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, newGmailUser.UserId.ToString()),
                new Claim(ClaimTypes.Name, newGmailUser.Username),
                new Claim(ClaimTypes.Email, newGmailUser.Email),
                new Claim(ClaimTypes.Role, roleCheck),
            };
            claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            HttpContext.Session.SetString("userPicture", profilePicture);
            return RedirectToAction("Index", "Home");
        }

        //[SessionAuthorize("User,ClubMember,ClubAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return  RedirectToAction("Login", "Account");
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
    }
}


