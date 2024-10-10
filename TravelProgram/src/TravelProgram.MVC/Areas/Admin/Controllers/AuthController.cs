using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.AuthVMs;

namespace TravelProgram.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(UserLoginVM vm)
        {
            if (!ModelState.IsValid) return View();

            LoginResponseVM data = null;

            try
            {
                data = await _authService.AdminLogin(vm);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "only admins can login");
                return View();
            }

            if (data == null)
            {
                ModelState.AddModelError("", "couldnt login2 admin");
                return View();
            }

            HttpContext.Response.Cookies.Append("token", data.AccessToken, new CookieOptions
            {
                Expires = data.ExpireDate,
                HttpOnly = true
            });

            return RedirectToAction("index", "home");
        }

        public IActionResult Logout()
        {
            _authService.Logout();

            return RedirectToAction("login");
        }
    }
}
