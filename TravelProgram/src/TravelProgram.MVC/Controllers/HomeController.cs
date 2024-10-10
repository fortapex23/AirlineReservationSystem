using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels;
using TravelProgram.MVC.ViewModels.AirlineVMs;
using TravelProgram.MVC.ViewModels.AirportVMs;
using TravelProgram.MVC.ViewModels.FlightVMs;

namespace TravelProgram.MVC.Controllers
{
	public class HomeController : Controller
	{
        private readonly ICrudService _crudService;
        private readonly HttpClient _httpClient;

        public HomeController(ICrudService crudService, HttpClient httpClient)
        {
            _crudService = crudService;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
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
            }

            var flights = await _crudService.GetAllAsync<List<FlightGetVM>>("/flights");

            var airports = await _crudService.GetAllAsync<List<AirportGetVM>>("/airports");

            var airlines = await _crudService.GetAllAsync<List<AirlineGetVM>>("/airlines");

            foreach (var flight in flights)
            {
                var departureAirport = airports.FirstOrDefault(x => x.Id == flight.DepartureAirportId);
                var arrivalAirport = airports.FirstOrDefault(x => x.Id == flight.ArrivalAirportId);
                var airline = airlines.FirstOrDefault(x => x.Id == flight.AirlineId);

                flight.DepartureAirportCity = departureAirport.City.ToString();
                flight.ArrivalAirportCity = arrivalAirport.City.ToString();
                flight.AirlineName = airline.Name;
                flight.AirlineCountry = airline.Country.ToString();
            }

            var homevm = new HomeVM()
            {
                FullName = ViewBag.FullName,
                Flights = flights,
            };

            return View(homevm);
        }


    }
}
