using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TravelProgram.MVC.ApiResponseMessages;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels;
using TravelProgram.MVC.ViewModels.AirlineVMs;
using TravelProgram.MVC.ViewModels.AirportVMs;
using TravelProgram.MVC.ViewModels.FlightVMs;
using TravelProgram.MVC.ViewModels.PlaneVMs;
using TravelProgram.MVC.ViewModels.WishListVMs;

namespace TravelProgram.MVC.Controllers
{
	public class HomeController : BaseController
	{
        private readonly ICrudService _crudService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HomeController(ICrudService crudService, HttpClient httpClient, IConfiguration configuration)
        {
            _crudService = crudService;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            SetFullName();

            var flights = await _crudService.GetAllAsync<List<FlightGetVM>>("/flights");
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
                    if (flight.Plane == null)
                    {
                        flight.Plane = new PlaneGetVM();
                    }

                    flight.PlaneName = plane.Name;

                    var airline = airlines.FirstOrDefault(x => x.Id == plane.AirlineId);
                    if (airline != null)
                    {
                        flight.Plane.AirlineName = airline.Name;
                    }
                }

                if (departureAirport != null)
                {
                    flight.DepartureAirportCity = departureAirport.City.ToString();
                }

                if (arrivalAirport != null)
                {
                    flight.ArrivalAirportCity = arrivalAirport.City.ToString();
                }
            }

            var homevm = new HomeVM()
            {
                FullName = ViewBag.FullName,
                Flights = flights?.Take(3).ToList(),
            };

            return View(homevm);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(int[] SelectedSeats)
        {
            if (SelectedSeats == null || !SelectedSeats.Any())
            {
                return BadRequest("No seats selected.");
            }

            string token = HttpContext.Request.Cookies["token"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("You must be logged in to add items to the basket.");
            }

            string appUserId = null;
            var secretKey = "sdfgdf-463dgdfsd j-fdvnji2387nTravel";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
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
                    return Unauthorized("User identity not found in the token.");
                }
            }
            catch (SecurityTokenException)
            {
                return Unauthorized("Invalid token. Please log in again.");
            }

            try
            {
                foreach (var seatId in SelectedSeats)
                {
                    var apiUrl = $"{_configuration.GetSection("Api:URL").Value}/basketitem?appUserId={appUserId}&seatId={seatId}";
                    var response = await _httpClient.PostAsync(apiUrl, null);

                    if (!response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", new { message = "Error adding seat to basket." });
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketItems()
        {
            string token = HttpContext.Request.Cookies["token"];
            string appUserId = null;
            ClaimsPrincipal claimsPrincipal = null;

            if (!string.IsNullOrEmpty(token))
            {
                var secretKey = "sdfgdf-463dgdfsd j-fdvnji2387nTravel";
                var key = Encoding.ASCII.GetBytes(secretKey);
                var tokenHandler = new JwtSecurityTokenHandler();

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

                    appUserId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }
                catch (SecurityTokenException)
                {
                    return Unauthorized("Invalid token.");
                }
            }

            if (!string.IsNullOrEmpty(appUserId))
            {
                try
                {
                    var apiUrl = $"{_configuration.GetSection("Api:URL").Value}/basketitem?appUserId={appUserId}";
                    var response = await _httpClient.GetStringAsync(apiUrl);

                    var apiResponse = JsonConvert.DeserializeObject<ApiResponseMessage<List<BasketItemVM>>>(response);

                    if (apiResponse != null && apiResponse.StatusCode == 200)
                    {
                        return Ok(apiResponse.Data);
                    }
                    else
                    {
                        return BadRequest(apiResponse.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                List<BasketItemVM> basketItemsVM = new List<BasketItemVM>();
                string basketStr = HttpContext.Request.Cookies["wishlist"];
                if (!string.IsNullOrEmpty(basketStr))
                {
                    basketItemsVM = JsonConvert.DeserializeObject<List<BasketItemVM>>(basketStr);
                }

                return Ok(basketItemsVM);
            }
        }

    }
}
