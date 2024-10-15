using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TravelProgram.MVC.ViewModels.WishListVMs;

namespace TravelProgram.MVC.Controllers
{
    public class WishListController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public WishListController(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            SetFullName();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromBasket(int flightId)
        {
            string token = HttpContext.Request.Cookies["token"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("You must be logged in to remove items from the basket");
            }

            string appUserId = null;
            ClaimsPrincipal claimsPrincipal = null;
            var secretKey = "sdfgdf-463dgdfsd j-fdvnji2387nTravel";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

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

                appUserId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(appUserId))
                {
                    return Unauthorized("User identity not found");
                }
            }
            catch (SecurityTokenException)
            {
                return Unauthorized("Invalid token. Please log in again.");
            }

            try
            {
                var apiUrl = $"{_configuration.GetSection("Api:URL").Value}/basketitem/remove?appUserId={appUserId}&flightId={flightId}";
                var response = await _httpClient.PostAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return BadRequest("Failed to remove flight from the basket.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

    }
}
