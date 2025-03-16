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
using Microsoft.EntityFrameworkCore;
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
      
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckLogin(string Gmail, string password)
        {
            var user = await _accountService.CheckLogin(Gmail, password);
            var adminGmail = _configuration["AdminAccount:Gmail"];
            var adminPassword = _configuration["AdminAccount:Password"];
            string? profilePicture = "";
            if (Gmail == adminGmail && password == adminPassword)
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
                    //profilePicture = Encoding.UTF8.GetString(user.ProfilePicture);
                    profilePicture = $"data:image/png;base64,{Convert.ToBase64String(user.ProfilePicture)}";

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
            //byte[] avatarByte = Encoding.UTF8.GetBytes(avatar);

            byte[] avatarByte;
            using (var httpClient = new HttpClient())
            {
                avatarByte = await httpClient.GetByteArrayAsync(avatar);
            }

            var claims = new List<Claim>();
            string profilePicture = "";
            ClaimsIdentity claimsIdentity;
            if (user != null)
            {
                var clubmember = await _accountService.CheckRole(user.UserId);
                //profilePicture = Encoding.UTF8.GetString(user.ProfilePicture);
                profilePicture = $"data:image/png;base64,{Convert.ToBase64String(user.ProfilePicture)}";
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
            //profilePicture = Encoding.UTF8.GetString(newGmailUser.ProfilePicture);
            profilePicture = $"data:image/png;base64,{Convert.ToBase64String(newGmailUser.ProfilePicture)}";
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
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return  RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            if (profilePicture != null && profilePicture.Length > 0)
            {
                // Validate file type (jpg, jpeg, png)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(profilePicture.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("profilePicture", "Only .jpg, .jpeg, and .png files are allowed.");
                    return View(); // Return the view with an error message
                }

                // Convert image to byte array
                byte[] profilePictureBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await profilePicture.CopyToAsync(memoryStream);
                    profilePictureBytes = memoryStream.ToArray();
                }

                // Get the currently authenticated user (you can adjust this based on your authentication)
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    // Save the image to the database
                    var user = await _accountService.FindUserAsync(int.Parse(userId));

                    if (user != null)
                    {
                        // Update the user's profile picture in the database
                        user.ProfilePicture = profilePictureBytes;
                        await _accountService.UpdateUserAsync(user);

                        // Optionally, update the session or return to the profile page with updated image
                        HttpContext.Session.SetString("userPicture", $"data:image/{fileExtension.TrimStart('.')};base64,{Convert.ToBase64String(profilePictureBytes)}");


                        return RedirectToAction("Edit", "Account");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("profilePicture", "No file selected.");
                return View(); // Return the view with an error message
            }

            return View();
        }
    }
}


