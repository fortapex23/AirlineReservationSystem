using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TravelProgram.MVC.Areas.Admin.ViewModels;

namespace TravelProgram.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
		private readonly HttpClient _httpClient;

		public HomeController(HttpClient httpClient)
        {
			_httpClient = httpClient;
		}

        public IActionResult Index()
        {
			string token = HttpContext.Request.Cookies["token"];
			if (token != null)
			{
				var secretKey = "sdfgdf-463dgdfsd j-fdvnji2387nTravel";
				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.ASCII.GetBytes(secretKey);
				ClaimsPrincipal claimsPrincipal = null;

				try
				{
					claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters()
					{
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(key)
					}, out SecurityToken validatedToken);
				}
				catch (SecurityTokenException)
				{

				}

				string fullname = claimsPrincipal?.Identity?.Name;

				if (fullname != null)
				{
					ViewBag.FullName = fullname;
				}

			};

			var adminvm = new AdminVM()
			{
				FullName = ViewBag.FullName,
			};

			return View(adminvm);
		}
    }
}
