﻿using System.Data;
using System.Security.Claims;
using System.Text;
using BussinessObjects.Models;
using BussinessObjects.Models.Dtos;
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
        private readonly IImageHelperService _imageService;
        public AccountController(IConfiguration configuration, IAccountService accountService, IImageHelperService imageHelperService)
        {
            _accountService = accountService;
            _configuration = configuration;
            _imageService = imageHelperService;
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
            if (user != null)
            {
                // login with admin role
                if (user.Email == adminGmail && user.Password == adminPassword)
                {
                    string role = "SystemAdmin";
                    var accountAdminId = user.UserId;
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



                var clubmember = await _accountService.CheckRole(user.UserId);

                if (user.ProfilePicture != null)
                {
                    //profilePicture = Encoding.UTF8.GetString(user.ProfilePicture);
                    profilePicture = _imageService.ConvertToBase64(user.ProfilePicture, "png");

                }
                // login with user role
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

                // login with ClubAdmin role
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

                // login with ClubMember role
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

            //Check if user exist in database
            if (user != null)
            {
                var clubmember = await _accountService.CheckRole(user.UserId);
                //profilePicture = Encoding.UTF8.GetString(user.ProfilePicture);
                profilePicture = $"data:image/png;base64,{Convert.ToBase64String(user.ProfilePicture)}";

                profilePicture = _imageService.ConvertToBase64(user.ProfilePicture, "png");
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
            // Add new user into database
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
            profilePicture = _imageService.ConvertToBase64(newGmailUser.ProfilePicture, "png");
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
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpUserDto newUser)
        {
            if (!ModelState.IsValid)
            {
                return View(newUser); 
            }
            bool isUserExist = CheckExistUser(newUser.Email, newUser.Username);

            if (isUserExist == true)
            {
                return RedirectToAction("SignUp", "Account");
            }
            byte[] defaultProfilePicture = await _accountService.GetDefaultProfilePictureAsync();
            User user = new User
            {
                Username = newUser.Username,
                Email = newUser.Email,
                Password = newUser.Password,
                CreatedAt = DateTime.Now,
                ProfilePicture = defaultProfilePicture
            };

            await _accountService.AddUser(user);

            TempData["SuccessMessage"] = "Sign up successfully! Please login.";
            return RedirectToAction("Login", "Account");
        }


        //[Authorize(Roles = "SystemAdmin")]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _accountService.FindUserAsync(id.Value);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}

        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _accountService.FindUserAsync(int.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            var editUserDto = new EditUserDto
            {
                Username = user.Username,
                CurrentPassword = user.Password,
                Email = user.Email
            };

            return View(editUserDto);
        }


        // POST: Save changes to profile
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserDto editUser)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var (success, message) = await _accountService.UpdateUserProfileAsync(userId, editUser);

            TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;
            return View(editUser);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            if (profilePicture != null && profilePicture.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(profilePicture.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("profilePicture", "Only .jpg, .jpeg, and .png files are allowed.");
                    return View();
                }

                // Convert image to byte array
                byte[] profilePictureBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await profilePicture.CopyToAsync(memoryStream);
                    profilePictureBytes = memoryStream.ToArray();
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    var user = await _accountService.FindUserAsync(int.Parse(userId));

                    if (user != null)
                    {
                        // Update  database
                        user.ProfilePicture = profilePictureBytes;
                        await _accountService.UpdateUserAsync(user);

                        var profilePictureBase64 = _imageService.ConvertToBase64(profilePictureBytes, fileExtension);
                        HttpContext.Session.SetString("userPicture", profilePictureBase64);

                        return RedirectToAction("Edit", "Account");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("profilePicture", "No file selected.");
                return View();
            }

            return View();
        }

        public bool CheckExistUser(string email, string username)
        {
            bool emailExists = _accountService.CheckEmailExist(email).Result != null;
            bool usernameExists = _accountService.CheckUsernameExist(username).Result != null;

            if (emailExists)
            {
                TempData["ErrorMessage"] = "Email already exists!";
                //return RedirectToAction("SignUp", "Account");
                return true;
            }
            if (usernameExists)
            {
                TempData["ErrorMessage"] = "Username already exists!";
                //return RedirectToAction("SignUp", "Account");
                return true;
            }

            return false;
        }
    }
}


