using Microsoft.AspNetCore.Mvc;

namespace ClubManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
