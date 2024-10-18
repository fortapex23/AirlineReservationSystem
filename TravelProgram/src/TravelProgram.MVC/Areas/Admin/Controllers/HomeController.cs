using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TravelProgram.MVC.Areas.Admin.ViewModels;
using TravelProgram.MVC.Controllers;

namespace TravelProgram.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class HomeController : BaseController
    {
		private readonly HttpClient _httpClient;

		public HomeController(HttpClient httpClient)
        {
			_httpClient = httpClient;
		}

        public IActionResult Index()
        {
			SetFullName();

			var adminvm = new AdminVM()
			{
				FullName = ViewBag.FullName,
			};

			return View(adminvm);
		}
    }
}
