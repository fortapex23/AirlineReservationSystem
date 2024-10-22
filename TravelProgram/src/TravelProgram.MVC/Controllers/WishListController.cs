using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.AirlineVMs;
using TravelProgram.MVC.ViewModels.AirportVMs;
using TravelProgram.MVC.ViewModels.FlightVMs;
using TravelProgram.MVC.ViewModels.PlaneVMs;
using TravelProgram.MVC.ViewModels.WishListVMs;

namespace TravelProgram.MVC.Controllers
{
    public class WishListController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ICrudService _crudService;

        public WishListController(IConfiguration configuration, HttpClient httpClient, ICrudService crudService)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _crudService = crudService;
        }

        public async Task<IActionResult> Index()
        {
            SetFullName();

            string appUserId = GetAppUserIdFromToken();
            if (string.IsNullOrEmpty(appUserId))
            {
                return Unauthorized("You must be logged in to view the wishlist.");
            }

            List<WishListVM> wishListItems;
            try
            {
                var apiUrl = $"{_configuration.GetSection("Api:URL").Value}/basketitem?appUserId={appUserId}";
                var response = await _httpClient.GetStringAsync(apiUrl);
                wishListItems = JsonConvert.DeserializeObject<List<WishListVM>>(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while fetching the wishlist: {ex.Message}");
            }

            if (wishListItems == null || wishListItems.Count == 0)
            {
                return View(new List<FlightGetVM>());
            }

            var flights = new List<FlightGetVM>();

            foreach (var wishListItem in wishListItems)
            {
                if (wishListItem.FlightId.HasValue)
                {
                    try
                    {
                        var flightApiUrl = $"{_configuration.GetSection("Api:URL").Value}/flights/{wishListItem.FlightId}";
                        var flightResponse = await _httpClient.GetStringAsync(flightApiUrl);
                        var flight = JsonConvert.DeserializeObject<FlightGetVM>(flightResponse);

                        if (flight != null)
                        {
                            flights.Add(flight);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest($"An error occurred while fetching flight details: {ex.Message}");
                    }
                }
            }

            var airports = await _crudService.GetAllAsync<List<AirportGetVM>>("/airports");
            var airlines = await _crudService.GetAllAsync<List<AirlineGetVM>>("/airlines");
            var planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

            foreach (var flight in flights)
            {
                var departureAirport = airports.FirstOrDefault(x => x.Id == flight.DepartureAirportId);
                var arrivalAirport = airports.FirstOrDefault(x => x.Id == flight.ArrivalAirportId);
                var plane = planes.FirstOrDefault(x => x.Id == flight.PlaneId);

                if (plane != null)
                {
                    flight.PlaneName = plane.Name;

                    var airline = airlines.FirstOrDefault(x => x.Id == plane.AirlineId);
                    if (airline != null)
                    {
                        flight.Plane.AirlineName = airline.Name;
                    }
                }

                flight.DepartureAirportCity = departureAirport?.City.ToString() ?? "dep city";
                flight.ArrivalAirportCity = arrivalAirport?.City.ToString() ?? "arr city";
            }

            return View(flights);
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

        private string GetAppUserIdFromToken()
        {
            string token = HttpContext.Request.Cookies["token"];
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var secretKey = "sdfgdf-463dgdfsd j-fdvnji2387nTravel";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
        }

    }
}
